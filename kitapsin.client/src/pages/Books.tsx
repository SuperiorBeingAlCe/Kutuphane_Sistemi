import React, { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

interface BookRaw {
    id: number;
    title: string;
    authorId: number;
    categoryId: number;
    publisherId: number;
    publicationYear: number;
    isbn: string;
    quantity: number;
    createdAt: string;
    isActive: boolean;
}

interface Book extends BookRaw {
    authorName: string;
    categoryName: string;
    publisherName: string;
}

function debounceAsync(
    func: (arg: string) => Promise<void>,
    delay: number
): (arg: string) => void {
    let timer: ReturnType<typeof setTimeout> | null = null;
    return (arg: string): void => {
        if (timer) clearTimeout(timer);
        timer = setTimeout(() => { func(arg).catch(console.error); }, delay);
    };
}

const Books: React.FC = () => {
    const [books, setBooks] = useState<Book[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [selectedBookId, setSelectedBookId] = useState<number | null>(null);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const navigate = useNavigate();

    // Toplam adet ve çeşit sayısı
    const totalQuantity = books.reduce((sum, b) => sum + b.quantity, 0);
    const totalTypes = books.length;

    // Helper functions...
    const fetchAuthorName = async (id: number): Promise<string> => {
        const token = localStorage.getItem('token') ?? '';
        const res = await fetch(`https://localhost:7195/api/Author/${id}`, { headers: { Authorization: `Bearer ${token}` } });
        if (!res.ok) throw new Error('Yazar detay alınamadı');
        const data = await res.json();
        return data.name;
    };
    const fetchCategoryName = async (id: number): Promise<string> => {
        const token = localStorage.getItem('token') ?? '';
        const res = await fetch(`https://localhost:7195/api/Category/${id}`, { headers: { Authorization: `Bearer ${token}` } });
        if (!res.ok) throw new Error('Kategori detay alınamadı');
        const data = await res.json();
        return data.name ?? data.title;
    };
    const fetchPublisherName = async (id: number): Promise<string> => {
        const token = localStorage.getItem('token') ?? '';
        const res = await fetch(`https://localhost:7195/api/Publisher/${id}`, { headers: { Authorization: `Bearer ${token}` } });
        if (!res.ok) throw new Error('Yayınevi detay alınamadı');
        const data = await res.json();
        return data.name;
    };

    const fetchBooks = useCallback(async (search: string = '') => {
        setLoading(true);
        setError(null);
        try {
            const token = localStorage.getItem('token') ?? '';
            const url = search
                ? `https://localhost:7195/api/Book/search?title=${encodeURIComponent(search)}`
                : 'https://localhost:7195/api/Book/all';
            const res = await fetch(url, { headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' } });
            if (!res.ok) throw new Error('Kitap verileri alınamadı');
            const data: BookRaw[] = await res.json();
            const enriched = await Promise.all(
                data.map(async b => ({
                    ...b,
                    authorName: await fetchAuthorName(b.authorId),
                    categoryName: await fetchCategoryName(b.categoryId),
                    publisherName: await fetchPublisherName(b.publisherId)
                }))
            );
            setBooks(enriched);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
        } finally {
            setLoading(false);
        }
    }, []);

    const debouncedFetchBooks = useCallback(debounceAsync(fetchBooks, 500), [fetchBooks]);

    useEffect(() => { fetchBooks(); }, [fetchBooks]);
    useEffect(() => {
        if (searchTerm.trim() === '') fetchBooks();
        else debouncedFetchBooks(searchTerm.trim());
    }, [searchTerm, fetchBooks, debouncedFetchBooks]);

    const handleDelete = async (id: number) => {
        if (!window.confirm('Bu kitabı silmek istediğinize emin misiniz?')) return;
        try {
            const token = localStorage.getItem('token') ?? '';
            const res = await fetch(`https://localhost:7195/api/Book/${id}`, { method: 'DELETE', headers: { Authorization: `Bearer ${token}` } });
            if (!res.ok) throw new Error('Silme işlemi başarısız');
            setBooks(prev => prev.filter(b => b.id !== id));
            if (selectedBookId === id) setSelectedBookId(null);
        } catch (err) {
            alert((err as Error).message);
        }
    };

    const handleUpdate = (id: number) => navigate(`/Books/UpdateBook/${id}`);
    const handleAdd = () => navigate('/Books/AddBook');
    const selectedBook = books.find(b => b.id === selectedBookId);

    return (
        <div style={containerStyle}>
            <div style={listStyle}>
                <div style={{ textAlign: 'right', marginBottom: 16 }}>
                    <button onClick={handleAdd} style={btnAddStyle}>Kitap Ekle</button>
                </div>
                <input
                    type="text"
                    placeholder="Kitap başlığı ara..."
                    value={searchTerm}
                    onChange={e => setSearchTerm(e.target.value)}
                    style={searchInputStyle}
                    autoFocus
                />
                {/* Toplam adet ve çeşit sayısı */}
                <p style={{ margin: '10px 0', fontWeight: 'bold' }}>Toplam Kitap Çeşidi: {totalTypes}</p>
                <p style={{ margin: '10px 0', fontWeight: 'bold' }}>Toplam Kitap Adedi: {totalQuantity}</p>
                {loading ? (
                    <div style={loadingStyle}>Yükleniyor...</div>
                ) : error ? (
                    <div style={errorStyle}>Hata: {error}</div>
                ) : (
                    <table style={tableStyle}>
                        <thead>
                            <tr>
                                <th style={thStyle}>#</th>
                                <th style={thStyle}>Başlık</th>
                                <th style={thStyle}>Adet</th>
                                <th style={thStyle}>Aktif</th>
                            </tr>
                        </thead>
                        <tbody>
                            {books.map((b, idx) => (
                                <tr
                                    key={b.id}
                                    onClick={() => setSelectedBookId(selectedBookId === b.id ? null : b.id)}
                                    style={{ backgroundColor: b.id === selectedBookId ? '#333' : 'transparent', cursor: 'pointer' }}
                                >
                                    <td style={tdStyle}>{idx + 1}</td>
                                    <td style={tdStyle}>{b.title}</td>
                                    <td style={tdStyle}>{b.quantity}</td>
                                    <td style={tdStyle}>{b.isActive ? 'Evet' : 'Hayır'}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
                <div style={{ ...panelStyle, right: selectedBook ? 40 : -9999 }}>
                    {selectedBook && (
                        <>
                            <button onClick={() => setSelectedBookId(null)} style={closeBtnStyle}>&times;</button>
                            <h3>Kitap Detayları</h3>
                            <p><strong>ID:</strong> {selectedBook.id}</p>
                            <p><strong>Başlık:</strong> {selectedBook.title}</p>
                            <p><strong>Yazar:</strong> {selectedBook.authorName}</p>
                            <p><strong>Kategori:</strong> {selectedBook.categoryName}</p>
                            <p><strong>Yayınevi:</strong> {selectedBook.publisherName}</p>
                            <p><strong>Basım Yılı:</strong> {selectedBook.publicationYear}</p>
                            <p><strong>ISBN:</strong> {selectedBook.isbn}</p>
                            <p><strong>Adet:</strong> {selectedBook.quantity}</p>
                            <p><strong>Eklenme:</strong> {new Date(selectedBook.createdAt).toLocaleDateString()}</p>
                            <p><strong>Aktif mi:</strong> {selectedBook.isActive ? 'Evet' : 'Hayır'}</p>
                            <div style={{ marginTop: 20, display: 'flex', gap: 10 }}>
                                <button onClick={() => handleDelete(selectedBook.id)} style={btnStyleDelete}>Sil</button>
                                <button onClick={() => handleUpdate(selectedBook.id)} style={btnStyleUpdate}>Güncelle</button>
                            </div>
                        </>
                    )}
                </div>
            </div>
        </div>
    );
};

// Stil objeleri
type CSS = React.CSSProperties;
const containerStyle: CSS = { width: '100vw', height: '100vh', backgroundColor: '#121212', color: '#fff', display: 'flex', alignItems: 'center', justifyContent: 'center', padding: 20 };
const listStyle: CSS = { width: '45vw', maxHeight: '90vh', overflowY: 'auto', backgroundColor: '#1e1e1e', borderRadius: 8, boxShadow: '0 0 15px rgba(0,0,0,0.7)', padding: 20 };
const tableStyle: CSS = { width: '100%', borderCollapse: 'collapse', color: '#fff' };
const thStyle: CSS = { borderBottom: '2px solid #444', padding: 10, backgroundColor: '#2e2e2e', textAlign: 'left' };
const tdStyle: CSS = { padding: 8, borderBottom: '1px solid #333' };
const btnAddStyle: CSS = { backgroundColor: '#0b74de', color: '#fff', padding: '8px 16px', border: 'none', borderRadius: 5, cursor: 'pointer', fontWeight: 'bold' };
const searchInputStyle: CSS = { padding: 8, marginBottom: 16, borderRadius: 5, border: 'none', fontSize: 16, width: '100%' };
const loadingStyle: CSS = { textAlign: 'center', fontSize: 18 };
const errorStyle: CSS = { color: 'red', textAlign: 'center', fontSize: 18 };
const panelStyle: CSS = { position: 'absolute', top: 20, width: '45vw', maxHeight: '90vh', backgroundColor: '#2a2a2a', borderRadius: 8, boxShadow: '0 0 20px rgba(0,0,0,0.9)', padding: 20, overflowY: 'auto', transition: 'right 0.3s ease', zIndex: 99 };
const closeBtnStyle: CSS = { alignSelf: 'flex-end', background: 'transparent', border: 'none', color: '#fff', fontSize: 24, cursor: 'pointer' };
const btnStyleDelete: CSS = { backgroundColor: '#e63946', border: 'none', color: '#fff', padding: '8px 14px', borderRadius: 4, cursor: 'pointer' };
const btnStyleUpdate: CSS = { backgroundColor: '#457b9d', border: 'none', color: '#fff', padding: '8px 14px', borderRadius: 4, cursor: 'pointer' };

export default Books;
