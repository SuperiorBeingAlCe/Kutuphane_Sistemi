import React, { useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

interface Option { id: number; name: string; }

function debounceAsync(
    func: (arg: string) => Promise<void>,
    delay: number
): (arg: string) => void {
    let timer: ReturnType<typeof setTimeout> | null = null;
    return (arg: string) => {
        if (timer) clearTimeout(timer);
        timer = setTimeout(() => { void func(arg); }, delay);
    };
}

const AddBook = () => {
    const [title, setTitle] = useState('');
    const [authorSearch, setAuthorSearch] = useState('');
    const [showAuthorDropdown, setShowAuthorDropdown] = useState(false);
    const [authorOptions, setAuthorOptions] = useState<Option[]>([]);
    const [authorId, setAuthorId] = useState<number | null>(null);

    const [categorySearch, setCategorySearch] = useState('');
    const [showCategoryDropdown, setShowCategoryDropdown] = useState(false);
    const [categoryOptions, setCategoryOptions] = useState<Option[]>([]);
    const [categoryId, setCategoryId] = useState<number | null>(null);

    const [publisherSearch, setPublisherSearch] = useState('');
    const [showPublisherDropdown, setShowPublisherDropdown] = useState(false);
    const [publisherOptions, setPublisherOptions] = useState<Option[]>([]);
    const [publisherId, setPublisherId] = useState<number | null>(null);

    const [publicationYear, setPublicationYear] = useState<number | ''>('');
    const [isbn, setIsbn] = useState('');
    const [quantity, setQuantity] = useState(1);

    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);

    const navigate = useNavigate();

    const fetchAuthors = useCallback(async (q: string) => {
        const token = localStorage.getItem('token');
        const url = q.trim()
            ? `https://localhost:7195/api/Author/search?name=${encodeURIComponent(q.trim())}`
            : 'https://localhost:7195/api/Author/all';
        const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
        if (!res.ok) throw new Error('Yazar verileri alınamadı');
        setAuthorOptions(await res.json());
    }, []);

    const fetchCategories = useCallback(async (q: string) => {
        const token = localStorage.getItem('token');
        const url = q.trim()
            ? `https://localhost:7195/api/Category/search?title=${encodeURIComponent(q.trim())}`
            : 'https://localhost:7195/api/Category/all';
        const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
        if (!res.ok) throw new Error('Kategori verileri alınamadı');
        setCategoryOptions(await res.json());
    }, []);

    const fetchPublishers = useCallback(async (q: string) => {
        const token = localStorage.getItem('token');
        const url = q.trim()
            ? `https://localhost:7195/api/Publisher/search?name=${encodeURIComponent(q.trim())}`
            : 'https://localhost:7195/api/Publisher';
        const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
        if (!res.ok) throw new Error('Yayınevi verileri alınamadı');
        setPublisherOptions(await res.json());
    }, []);

    const debouncedAuthors = useCallback(debounceAsync(fetchAuthors, 500), [fetchAuthors]);
    const debouncedCategories = useCallback(debounceAsync(fetchCategories, 500), [fetchCategories]);
    const debouncedPublishers = useCallback(debounceAsync(fetchPublishers, 500), [fetchPublishers]);

    const onAuthorFocus = () => {
        setShowAuthorDropdown(true);
        debouncedAuthors(authorSearch);
    };
    const onAuthorChange = (q: string) => {
        setAuthorSearch(q);
        setShowAuthorDropdown(true);
        debouncedAuthors(q);
    };

    const onCategoryFocus = () => {
        setShowCategoryDropdown(true);
        debouncedCategories(categorySearch);
    };
    const onCategoryChange = (q: string) => {
        setCategorySearch(q);
        setShowCategoryDropdown(true);
        debouncedCategories(q);
    };

    const onPublisherFocus = () => {
        setShowPublisherDropdown(true);
        debouncedPublishers(publisherSearch);
    };
    const onPublisherChange = (q: string) => {
        setPublisherSearch(q);
        setShowPublisherDropdown(true);
        debouncedPublishers(q);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (authorId === null || categoryId === null || publisherId === null) {
            setError('Lütfen tüm seçimleri yapın.');
            return;
        }
        try {
            const token = localStorage.getItem('token');
            const payload = { title, authorId, categoryId, publisherId, publicationYear: Number(publicationYear), isbn, quantity };
            await axios.post('https://localhost:7195/api/Book', payload, {
                headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' }
            });
            setSuccess(true);
            setError(null);
            setTimeout(() => navigate('/Books'), 1500);
        } catch (err) {
            setSuccess(false);
            setError(err instanceof Error ? err.message : 'Kitap eklenemedi');
        }
    };

    return (
        <div style={wrapperStyle}>
            <form onSubmit={handleSubmit} style={formStyle}>
                <h2 style={{ textAlign: 'center', marginBottom: 20 }}>Kitap Ekle</h2>

                <div style={fieldStyle}>
                    <label>Başlık *</label>
                    <input type="text" value={title} onChange={e => setTitle(e.target.value)} required style={inputStyle} />
                </div>

                <div style={fieldStyle}>
                    <label>Yazar Ara *</label>
                    <input
                        type="text"
                        value={authorSearch}
                        onFocus={onAuthorFocus}
                        onBlur={() => setTimeout(() => setShowAuthorDropdown(false), 150)}
                        onChange={e => onAuthorChange(e.target.value)}
                        placeholder="Yazar adı..."
                        style={inputStyle}
                    />
                    {showAuthorDropdown && (
                        <ul style={dropdownStyle}>
                            {authorOptions.map(a => (
                                <li key={a.id} onMouseDown={() => { setAuthorId(a.id); setAuthorSearch(a.name); }} style={itemStyle}>
                                    {a.name}
                                </li>
                            ))}
                        </ul>
                    )}
                </div>

                <div style={fieldStyle}>
                    <label>Kategori Ara *</label>
                    <input
                        type="text"
                        value={categorySearch}
                        onFocus={onCategoryFocus}
                        onBlur={() => setTimeout(() => setShowCategoryDropdown(false), 150)}
                        onChange={e => onCategoryChange(e.target.value)}
                        placeholder="Kategori adı..."
                        style={inputStyle}
                    />
                    {showCategoryDropdown && (
                        <ul style={dropdownStyle}>
                            {categoryOptions.map(c => (
                                <li key={c.id} onMouseDown={() => { setCategoryId(c.id); setCategorySearch(c.name); }} style={itemStyle}>
                                    {c.name}
                                </li>
                            ))}
                        </ul>
                    )}
                </div>

                <div style={fieldStyle}>
                    <label>Yayınevi Ara *</label>
                    <input
                        type="text"
                        value={publisherSearch}
                        onFocus={onPublisherFocus}
                        onBlur={() => setTimeout(() => setShowPublisherDropdown(false), 150)}
                        onChange={e => onPublisherChange(e.target.value)}
                        placeholder="Yayınevi adı..."
                        style={inputStyle}
                    />
                    {showPublisherDropdown && (
                        <ul style={dropdownStyle}>
                            {publisherOptions.map(p => (
                                <li key={p.id} onMouseDown={() => { setPublisherId(p.id); setPublisherSearch(p.name); }} style={itemStyle}>
                                    {p.name}
                                </li>
                            ))}
                        </ul>
                    )}
                </div>

                <div style={fieldStyle}>
                    <label>Basım Yılı *</label>
                    <input type="number" value={publicationYear} onChange={e => setPublicationYear(e.target.value === '' ? '' : Number(e.target.value))} required style={inputStyle} />
                </div>
                <div style={fieldStyle}>
                    <label>ISBN</label>
                    <input type="text" value={isbn} onChange={e => setIsbn(e.target.value)} style={inputStyle} />
                </div>
                <div style={fieldStyle}>
                    <label>Adet *</label>
                    <input type="number" value={quantity} onChange={e => setQuantity(Number(e.target.value))} required min={1} style={inputStyle} />
                </div>

                {error && <p style={{ color: 'red' }}>{error}</p>}
                {success && <p style={{ color: 'lightgreen' }}>Kitap başarıyla eklendi!</p>}

                <button type="submit" style={btnStyle}>Kaydet</button>
            </form>
        </div>
    );
};

const wrapperStyle: React.CSSProperties = {
    width: '100%',
    minHeight: '100vh',
    backgroundColor: '#121212',
    color: '#fff',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'flex-start',
    padding: '40px 20px',
    boxSizing: 'border-box'
};
const formStyle: React.CSSProperties = { backgroundColor: '#1e1e1e', padding: 20, borderRadius: 8, boxShadow: '0 0 15px rgba(0,0,0,0.7)', width: 320 };
const fieldStyle: React.CSSProperties = { marginBottom: 16, display: 'flex', flexDirection: 'column' };
const inputStyle: React.CSSProperties = { padding: 8, borderRadius: 4, border: '1px solid #555', backgroundColor: '#2a2a2a', color: '#fff' };
const dropdownStyle: React.CSSProperties = { listStyle: 'none', maxHeight: 120, overflowY: 'auto', padding: 0, margin: '4px 0', backgroundColor: '#333', borderRadius: 4 };
const itemStyle: React.CSSProperties = { padding: 8, cursor: 'pointer' };
const btnStyle: React.CSSProperties = { padding: 10, backgroundColor: '#0b74de', border: 'none', color: '#fff', fontWeight: 'bold', borderRadius: 5, cursor: 'pointer' };

export default AddBook;
