import React, { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

interface Author {
    id: number;
    name: string;
}

function debounceAsync(
    func: (arg: string) => Promise<void>,
    delay: number
): (arg: string) => void {
    let timer: ReturnType<typeof setTimeout> | null = null;

    return (arg: string): void => {
        if (timer) clearTimeout(timer);
        timer = setTimeout(() => {
            func(arg).catch(console.error);
        }, delay);
    };
}

const Authors: React.FC = () => {
    const [authors, setAuthors] = useState<Author[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [selectedAuthorId, setSelectedAuthorId] = useState<number | null>(null);
    const [searchTerm, setSearchTerm] = useState<string>('');

    const navigate = useNavigate();

    const fetchAuthors = useCallback(async (search: string = '') => {
        setLoading(true);
        setError(null);

        try {
            
            const token = localStorage.getItem('token') ?? '';
            const url = search
                ? `https://localhost:7195/api/Author/search?name=${encodeURIComponent(search)}`
                : 'https://localhost:7195/api/Author/all';

            const res = await fetch(url, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
            });

            if (!res.ok) throw new Error('Yazar verileri alınamadı.');

            const data: Author[] = await res.json();
            setAuthors(data);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
        } finally {
            setLoading(false);
        }
    }, []);

    const debouncedFetchAuthors = useCallback(debounceAsync(fetchAuthors, 500), [fetchAuthors]);

    useEffect(() => {
        fetchAuthors();
    }, [fetchAuthors]);

    useEffect(() => {
        if (searchTerm.trim() === '') {
            fetchAuthors();
        } else {
            debouncedFetchAuthors(searchTerm);
        }
    }, [searchTerm, fetchAuthors, debouncedFetchAuthors]);

    const handleDelete = async (id: number) => {
        if (!window.confirm('Bu yazarı silmek istediğinize emin misiniz?')) return;

        try {
            const token = localStorage.getItem('token') ?? '';
            const res = await fetch(`https://localhost:7195/api/Author/${id}`, {
                method: 'DELETE',
                headers: { Authorization: `Bearer ${token}` },
            });

            if (!res.ok) throw new Error('Silme işlemi başarısız.');

            setAuthors((prev) => prev.filter((a) => a.id !== id));
            if (selectedAuthorId === id) setSelectedAuthorId(null);
        } catch (err) {
            alert(`Silme sırasında hata oluştu: ${(err as Error).message}`);
        }
    };

    const handleUpdate = (id: number) => {
        navigate(`/Authors/UpdateAuthor/${id}`);
    };

    const selectedAuthor = authors.find((a) => a.id === selectedAuthorId);

    return (
        <div style={containerStyle}>
            <div style={boxStyle}>
                <div style={{ marginBottom: 16, textAlign: 'right' }}>
                    <button onClick={() => navigate('/authors/AddAuthor')} style={btnAddStyle}>
                        Yazar Ekle
                    </button>
                </div>

                <input
                    type="text"
                    placeholder="Yazar ara..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    style={searchInputStyle}
                    autoFocus
                />

                {loading ? (
                    <div style={{ textAlign: 'center', fontSize: 18 }}>Yükleniyor...</div>
                ) : error ? (
                    <div style={errorStyle}>Hata oluştu: {error}</div>
                ) : (
                    <>
                        <h2 style={{ marginBottom: 16, textAlign: 'center' }}>Yazar Listesi</h2>
                        <table style={tableStyle}>
                            <thead>
                                <tr>
                                    <th style={thStyle}>#</th>
                                    <th style={thStyle}>Yazar Adı</th>
                                    <th style={thStyle}>İşlem</th>
                                </tr>
                            </thead>
                            <tbody>
                                {authors.map((author, index) => (
                                    <tr
                                        key={author.id}
                                        onClick={() =>
                                            setSelectedAuthorId(
                                                selectedAuthorId === author.id ? null : author.id
                                            )
                                        }
                                        style={{
                                            backgroundColor:
                                                author.id === selectedAuthorId ? '#333' : 'transparent',
                                            cursor: 'pointer',
                                        }}
                                    >
                                        <td style={tdStyle}>{index + 1}</td>
                                        <td style={tdStyle}>{author.name}</td>
                                        <td style={tdStyle}>
                                            <button
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    handleDelete(author.id);
                                                }}
                                                style={btnStyleDelete}
                                            >
                                                Sil
                                            </button>
                                            <button
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    handleUpdate(author.id);
                                                }}
                                                style={btnStyleUpdate}
                                            >
                                                Güncelle
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>

                        {selectedAuthor && (
                            <div style={selectedAuthorStyle}>
                                <h3>Seçilen Yazar Detayları</h3>
                                <p>
                                    <strong>ID:</strong> {selectedAuthor.id}
                                </p>
                                <p>
                                    <strong>Ad:</strong> {selectedAuthor.name}
                                </p>
                            </div>
                        )}
                    </>
                )}
            </div>
        </div>
    );
};

// Stil objeleri
const containerStyle: React.CSSProperties = {
    width: '100vw',
    height: '100vh',
    backgroundColor: '#121212',
    color: '#fff',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
    boxSizing: 'border-box',
    userSelect: 'none',
};

const boxStyle: React.CSSProperties = {
    width: '50vw',
    maxHeight: '50vh',
    backgroundColor: '#1e1e1e',
    borderRadius: 8,
    boxShadow: '0 0 15px rgba(0,0,0,0.7)',
    padding: 20,
    display: 'flex',
    flexDirection: 'column',
    overflowY: 'auto',
};

const btnAddStyle: React.CSSProperties = {
    backgroundColor: '#0b74de',
    border: 'none',
    color: 'white',
    padding: '8px 16px',
    borderRadius: 5,
    cursor: 'pointer',
    fontWeight: 'bold',
};

const searchInputStyle: React.CSSProperties = {
    padding: 8,
    marginBottom: 16,
    borderRadius: 5,
    border: 'none',
    fontSize: 16,
};

const errorStyle: React.CSSProperties = {
    color: 'red',
    fontWeight: 'bold',
    textAlign: 'center',
    fontSize: 18,
    padding: '30px 0',
};

const tableStyle: React.CSSProperties = {
    width: '100%',
    borderCollapse: 'collapse',
    color: '#fff',
};

const thStyle: React.CSSProperties = {
    borderBottom: '2px solid #444',
    padding: 10,
    textAlign: 'left',
    backgroundColor: '#2e2e2e',
};

const tdStyle: React.CSSProperties = {
    padding: 8,
    borderBottom: '1px solid #333',
};

const btnStyleDelete: React.CSSProperties = {
    backgroundColor: '#e63946',
    border: 'none',
    color: 'white',
    padding: '5px 10px',
    marginRight: 8,
    borderRadius: 4,
    cursor: 'pointer',
    fontWeight: 600,
    fontSize: 12,
};

const btnStyleUpdate: React.CSSProperties = {
    backgroundColor: '#457b9d',
    border: 'none',
    color: 'white',
    padding: '5px 10px',
    borderRadius: 4,
    cursor: 'pointer',
    fontWeight: 600,
    fontSize: 12,
};

const selectedAuthorStyle: React.CSSProperties = {
    marginTop: 20,
    padding: 15,
    backgroundColor: '#2a2a2a',
    borderRadius: 6,
    color: '#ddd',
    fontSize: 14,
};

export default Authors;