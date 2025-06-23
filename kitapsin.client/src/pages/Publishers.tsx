import React, { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

interface Publisher {
    id: number;
    name: string;
    bookId?: number | null;
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

const Publishers: React.FC = () => {
    const [publishers, setPublishers] = useState<Publisher[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [selectedPublisherId, setSelectedPublisherId] = useState<number | null>(null);
    const [searchTerm, setSearchTerm] = useState<string>('');

    const navigate = useNavigate();

    const fetchPublishers = useCallback(async (search: string = '') => {
        setLoading(true);
        setError(null);

        try {
            const token = localStorage.getItem('token') ?? '';
            const url = search
                ? `https://localhost:7195/api/Publisher/search?name=${encodeURIComponent(search)}`
                : 'https://localhost:7195/api/Publisher';

            const res = await fetch(url, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
            });

            if (!res.ok) throw new Error('Publisher verileri alınamadı.');

            const data: Publisher[] = await res.json();
            setPublishers(data);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
        } finally {
            setLoading(false);
        }
    }, []);

    const debouncedFetchPublishers = useCallback(debounceAsync(fetchPublishers, 500), [fetchPublishers]);

    useEffect(() => {
        fetchPublishers();
    }, [fetchPublishers]);

    useEffect(() => {
        if (searchTerm.trim() === '') {
            fetchPublishers();
        } else {
            debouncedFetchPublishers(searchTerm);
        }
    }, [searchTerm, fetchPublishers, debouncedFetchPublishers]);

    const handleDelete = async (id: number) => {
        if (!window.confirm('Bu yayınevini silmek istediğinize emin misiniz?')) return;

        try {
            const token = localStorage.getItem('token') ?? '';
            const res = await fetch(`https://localhost:7195/api/Publisher/${id}`, {
                method: 'DELETE',
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (!res.ok) throw new Error('Silme işlemi başarısız.');

            setPublishers((prev) => prev.filter((p) => p.id !== id));
            if (selectedPublisherId === id) setSelectedPublisherId(null);
        } catch (err) {
            alert(`Silme sırasında hata oluştu: ${(err as Error).message}`);
        }
    };

    const handleUpdate = (id: number) => {
        navigate(`/publishers/update/${id}`);
    };

    const selectedPublisher = publishers.find((p) => p.id === selectedPublisherId);

    return (
        <div style={containerStyle}>
            <div style={boxStyle}>
                <div style={{ marginBottom: 16, textAlign: 'right' }}>
                    <button onClick={() => navigate('/Publishers/AddPublisher')} style={btnAddStyle}>
                        Yayınevi Ekle
                    </button>
                </div>

                {/* Arama çubuğu */}
                <input
                    type="text"
                    placeholder="Yayınevi ara..."
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
                        <h2 style={{ marginBottom: 16, textAlign: 'center' }}>Yayınevi Listesi</h2>
                        <table style={tableStyle}>
                            <thead>
                                <tr>
                                    <th style={thStyle}>ID</th>
                                    <th style={thStyle}>Yayınevi Adı</th>
                                    <th style={thStyle}>İşlem</th>
                                </tr>
                            </thead>
                            <tbody>
                                {publishers.map((publisher) => (
                                    <tr
                                        key={publisher.id}
                                        onClick={() =>
                                            setSelectedPublisherId(
                                                selectedPublisherId === publisher.id ? null : publisher.id
                                            )
                                        }
                                        style={{
                                            backgroundColor: publisher.id === selectedPublisherId ? '#333' : 'transparent',
                                            cursor: 'pointer',
                                        }}
                                    >
                                        <td style={tdStyle}>{publisher.id}</td>
                                        <td style={tdStyle}>{publisher.name}</td>
                                        <td style={tdStyle}>
                                            <button
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    handleDelete(publisher.id);
                                                }}
                                                style={btnStyleDelete}
                                            >
                                                Sil
                                            </button>
                                            <button
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    handleUpdate(publisher.id);
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

                        {selectedPublisher && (
                            <div style={selectedCategoryStyle}>
                                <h3>Seçilen Yayınevi Detayları</h3>
                                <p>
                                    <strong>ID:</strong> {selectedPublisher.id}
                                </p>
                                <p>
                                    <strong>Ad:</strong> {selectedPublisher.name}
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

const selectedCategoryStyle: React.CSSProperties = {
    marginTop: 20,
    padding: 15,
    backgroundColor: '#2a2a2a',
    borderRadius: 6,
    color: '#ddd',
    fontSize: 14,
};

export default Publishers;