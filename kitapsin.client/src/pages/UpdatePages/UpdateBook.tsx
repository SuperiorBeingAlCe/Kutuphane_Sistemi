import React, { useEffect, useState, useCallback } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';

interface Option { id: number; name: string; }

interface BookData {
    title: string;
    authorId: number;
    categoryId: number;
    publisherId: number;
    publicationYear: number;
    isbn: string;
    quantity: number;
    isActive: boolean;
    authorName?: string;
    categoryName?: string;
    publisherName?: string;
}

function debounceAsync(func: (arg: string) => Promise<void>, delay: number): (arg: string) => void {
    let timer: ReturnType<typeof setTimeout> | null = null;
    return (arg: string) => {
        if (timer) clearTimeout(timer);
        timer = setTimeout(() => { void func(arg); }, delay);
    };
}

const UpdateBook: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [book, setBook] = useState<BookData | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);

    const [authorSearch, setAuthorSearch] = useState('');
    const [categorySearch, setCategorySearch] = useState('');
    const [publisherSearch, setPublisherSearch] = useState('');

    const [authorOptions, setAuthorOptions] = useState<Option[]>([]);
    const [categoryOptions, setCategoryOptions] = useState<Option[]>([]);
    const [publisherOptions, setPublisherOptions] = useState<Option[]>([]);

    // Kitap verisini çek
    const fetchBook = async () => {
        try {
            const token = localStorage.getItem('token');
            const res = await axios.get<BookData>(`https://localhost:7195/api/Book/${id}`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            setBook(res.data);
            setAuthorSearch(res.data.authorName || '');
            setCategorySearch(res.data.categoryName || '');
            setPublisherSearch(res.data.publisherName || '');
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
        }
    };

    useEffect(() => {
        if (id) fetchBook();
    }, [id]);

    // Yazarları getir
    const fetchAuthors = useCallback(async (q: string) => {
        try {
            const token = localStorage.getItem('token');
            const url = q.trim()
                ? `https://localhost:7195/api/Author/search?title=${encodeURIComponent(q.trim())}`
                : 'https://localhost:7195/api/Author/all';
            const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
            if (!res.ok) throw new Error('Yazar verileri alınamadı');
            setAuthorOptions(await res.json());
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
        }
    }, []);

    // Kategorileri getir
    const fetchCategories = useCallback(async (q: string) => {
        try {
            const token = localStorage.getItem('token');
            const url = q.trim()
                ? `https://localhost:7195/api/Category/search?title=${encodeURIComponent(q.trim())}`
                : 'https://localhost:7195/api/Category/all';
            const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
            if (!res.ok) throw new Error('Kategori verileri alınamadı');
            setCategoryOptions(await res.json());
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
        }
    }, []);

    // Yayıncıları getir
    const fetchPublishers = useCallback(async (q: string) => {
        try {
            const token = localStorage.getItem('token');
            const url = q.trim()
                ? `https://localhost:7195/api/Publisher/search?name=${encodeURIComponent(q.trim())}`
                : 'https://localhost:7195/api/Publisher';
            const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
            if (!res.ok) throw new Error('Yayınevi verileri alınamadı');
            setPublisherOptions(await res.json());
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
        }
    }, []);

    // Debounce ile arama
    const debouncedAuthor = useCallback(debounceAsync(fetchAuthors, 500), [fetchAuthors]);
    const debouncedCategory = useCallback(debounceAsync(fetchCategories, 500), [fetchCategories]);
    const debouncedPublisher = useCallback(debounceAsync(fetchPublishers, 500), [fetchPublishers]);

    // Güncelleme işlemi
    const handleUpdate = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!book) return;
        if (!book.title.trim() || !book.authorId || !book.categoryId || !book.publisherId) {
            setError('Lütfen tüm zorunlu alanları doldurun.');
            return;
        }
        try {
            setError(null);
            const token = localStorage.getItem('token');
            await axios.put(`https://localhost:7195/api/Book/${id}`, book, {
                headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' }
            });
            setSuccess(true);
            setTimeout(() => navigate('/Books'), 1500);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
        }
    };

    if (!book) return <div style={loadingStyle}>Yükleniyor...</div>;

    return (
        <div style={pageStyle}>
            <form onSubmit={handleUpdate} style={formStyle}>
                <h2 style={titleStyle}>Kitap Güncelle</h2>

                <label style={labelStyle}>
                    Başlık:
                    <input
                        style={inputStyle}
                        value={book.title}
                        onChange={e => setBook({ ...book, title: e.target.value })}
                        required
                        placeholder="Başlık"
                    />
                </label>

                <label style={labelStyle}>
                    Yazar Ara:
                    <input
                        style={inputStyle}
                        value={authorSearch}
                        onChange={e => { setAuthorSearch(e.target.value); debouncedAuthor(e.target.value); }}
                        onFocus={() => debouncedAuthor(authorSearch)}
                        placeholder="Yazar adı"
                        required
                    />
                    <ul style={dropdownStyle}>
                        {authorOptions.map(opt => (
                            <li
                                key={opt.id}
                                style={itemStyle}
                                onClick={() => {
                                    setBook({ ...book, authorId: opt.id });
                                    setAuthorSearch(opt.name);
                                    setAuthorOptions([]);
                                }}
                            >
                                {opt.name}
                            </li>
                        ))}
                    </ul>
                </label>

                <label style={labelStyle}>
                    Kategori Ara:
                    <input
                        style={inputStyle}
                        value={categorySearch}
                        onChange={e => { setCategorySearch(e.target.value); debouncedCategory(e.target.value); }}
                        onFocus={() => debouncedCategory(categorySearch)}
                        placeholder="Kategori adı"
                        required
                    />
                    <ul style={dropdownStyle}>
                        {categoryOptions.map(opt => (
                            <li
                                key={opt.id}
                                style={itemStyle}
                                onClick={() => {
                                    setBook({ ...book, categoryId: opt.id });
                                    setCategorySearch(opt.name);
                                    setCategoryOptions([]);
                                }}
                            >
                                {opt.name}
                            </li>
                        ))}
                    </ul>
                </label>

                <label style={labelStyle}>
                    Yayıncı Ara:
                    <input
                        style={inputStyle}
                        value={publisherSearch}
                        onChange={e => { setPublisherSearch(e.target.value); debouncedPublisher(e.target.value); }}
                        onFocus={() => debouncedPublisher(publisherSearch)}
                        placeholder="Yayıncı adı"
                        required
                    />
                    <ul style={dropdownStyle}>
                        {publisherOptions.map(opt => (
                            <li
                                key={opt.id}
                                style={itemStyle}
                                onClick={() => {
                                    setBook({ ...book, publisherId: opt.id });
                                    setPublisherSearch(opt.name);
                                    setPublisherOptions([]);
                                }}
                            >
                                {opt.name}
                            </li>
                        ))}
                    </ul>
                </label>

                <label style={labelStyle}>
                    Basım Yılı:
                    <input
                        style={inputStyle}
                        type="number"
                        value={book.publicationYear}
                        onChange={e => setBook({ ...book, publicationYear: Number(e.target.value) })}
                        required
                    />
                </label>

                <label style={labelStyle}>
                    ISBN:
                    <input
                        style={inputStyle}
                        value={book.isbn}
                        onChange={e => setBook({ ...book, isbn: e.target.value })}
                    />
                </label>

                <label style={labelStyle}>
                    Adet:
                    <input
                        style={inputStyle}
                        type="number"
                        value={book.quantity}
                        min={1}
                        onChange={e => setBook({ ...book, quantity: Number(e.target.value) })}
                        required
                    />
                </label>

                <label style={labelStyle}>
                    Aktif mi?
                    <input
                        style={{ marginLeft: '8px' }}
                        type="checkbox"
                        checked={book.isActive}
                        onChange={e => setBook({ ...book, isActive: e.target.checked })}
                    />
                </label>

                {error && <div style={errorStyle}>Hata: {error}</div>}
                {success && <div style={successStyle}>Güncelleme başarılı!</div>}

                <button type="submit" style={buttonStyle}>Kaydet</button>
            </form>
        </div>
    );
};

// Stil sabitleri
const pageStyle: React.CSSProperties = {
    height: '100vh',
    backgroundColor: '#121212',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    padding: '20px',
};

const formStyle: React.CSSProperties = {
    backgroundColor: '#1e1e1e',
    padding: '30px',
    borderRadius: '10px',
    width: '100%',
    maxWidth: '450px',
    display: 'flex',
    flexDirection: 'column',
    gap: '15px',
    boxShadow: '0 0 15px rgba(0,0,0,0.7)',
};

const titleStyle: React.CSSProperties = {
    color: '#fff',
    textAlign: 'center',
    marginBottom: '10px',
};

const labelStyle: React.CSSProperties = {
    display: 'flex',
    flexDirection: 'column',
    color: '#ccc',
    fontSize: '14px',
    fontWeight: 'bold',
};

const inputStyle: React.CSSProperties = {
    marginTop: '5px',
    padding: '10px',
    borderRadius: '5px',
    border: 'none',
    backgroundColor: '#2a2a2a',
    color: '#fff',
};

const dropdownStyle: React.CSSProperties = {
    marginTop: '5px',
    listStyle: 'none',
    padding: 0,
    maxHeight: '120px',
    overflowY: 'auto',
    backgroundColor: '#2a2a2a',
    borderRadius: '5px',
};

const itemStyle: React.CSSProperties = {
    padding: '8px 10px',
    cursor: 'pointer',
    borderBottom: '1px solid #333',
    color: '#fff',
};

const buttonStyle: React.CSSProperties = {
    marginTop: '10px',
    padding: '12px',
    backgroundColor: '#0b74de',
    color: '#fff',
    border: 'none',
    borderRadius: '5px',
    fontWeight: 'bold',
    cursor: 'pointer',
};

const loadingStyle: React.CSSProperties = {
    color: '#ccc',
    fontSize: '18px',
};

const errorStyle: React.CSSProperties = {
    color: '#e63946',
    fontSize: '14px',
    textAlign: 'center',
};

const successStyle: React.CSSProperties = {
    color: '#38a169',
    fontSize: '14px',
    textAlign: 'center',
};

export default UpdateBook;
