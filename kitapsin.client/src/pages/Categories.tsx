import React, { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

interface Category {
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

const Categories: React.FC = () => {
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [selectedCategoryId, setSelectedCategoryId] = useState<number | null>(null);
    const [searchTerm, setSearchTerm] = useState<string>('');

    const navigate = useNavigate();

    const fetchCategories = useCallback(async (search: string = '') => {
        setLoading(true);
        setError(null);

        try {
            const token = localStorage.getItem('token') ?? '';

            // API endpoint, arama varsa ona göre ayarla
            const url = search
                ? `https://localhost:7195/api/Category/search?title=${encodeURIComponent(search)}`
                : 'https://localhost:7195/api/Category/all';

            const res = await fetch(url, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
            });

            if (!res.ok) throw new Error('Kategori verileri alınamadı.');

            const data: Category[] = await res.json();
            setCategories(data);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
        } finally {
            setLoading(false);
        }
    }, []);

  
    const debouncedFetchCategories = useCallback(debounceAsync(fetchCategories, 500), [fetchCategories]);

    
    useEffect(() => {
        fetchCategories();
    }, [fetchCategories]);

    
    useEffect(() => {
        if (searchTerm.trim() === '') {
            fetchCategories();
        } else {
            debouncedFetchCategories(searchTerm);
        }
    }, [searchTerm, fetchCategories, debouncedFetchCategories]);

    const handleDelete = async (id: number) => {
        if (!window.confirm('Bu kategoriyi silmek istediğinize emin misiniz?')) return;

        try {
            const token = localStorage.getItem('token') ?? '';
            const res = await fetch(`https://localhost:7195/api/Category/${id}`, {
                method: 'DELETE',
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (!res.ok) throw new Error('Silme işlemi başarısız.');

            setCategories((prev) => prev.filter((c) => c.id !== id));
            if (selectedCategoryId === id) setSelectedCategoryId(null);
        } catch (err) {
            alert(`Silme sırasında hata oluştu: ${(err as Error).message}`);
        }
    };

    const handleUpdate = (id: number) => {
        navigate(`/categories/update/${id}`);
    };

    const selectedCategory = categories.find((c) => c.id === selectedCategoryId);

    return (
        <div style={containerStyle}>
            <div style={boxStyle}>
                <div style={{ marginBottom: 16, textAlign: 'right' }}>
                    <button onClick={() => navigate('/categories/add')} style={btnAddStyle}>
                        Kategori Ekle
                    </button>
                </div>

                {/* Arama çubuğu */}
                <input
                    type="text"
                    placeholder="Kategori ara..."
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
                        <h2 style={{ marginBottom: 16, textAlign: 'center' }}>Kategori Listesi</h2>
                        <table style={tableStyle}>
                            <thead>
                                <tr>
                                    <th style={thStyle}>ID</th>
                                    <th style={thStyle}>Kategori Adı</th>
                                    <th style={thStyle}>İşlem</th>
                                </tr>
                            </thead>
                            <tbody>
                                {categories.map((category) => (
                                    <tr
                                        key={category.id}
                                        onClick={() =>
                                            setSelectedCategoryId(selectedCategoryId === category.id ? null : category.id)
                                        }
                                        style={{
                                            backgroundColor: category.id === selectedCategoryId ? '#333' : 'transparent',
                                            cursor: 'pointer',
                                        }}
                                    >
                                        <td style={tdStyle}>{category.id}</td>
                                        <td style={tdStyle}>{category.name}</td>
                                        <td style={tdStyle}>
                                            <button
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    handleDelete(category.id);
                                                }}
                                                style={btnStyleDelete}
                                            >
                                                Sil
                                            </button>
                                            <button
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    handleUpdate(category.id);
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

                        {selectedCategory && (
                            <div style={selectedCategoryStyle}>
                                <h3>Seçilen Kategori Detayları</h3>
                                <p>
                                    <strong>ID:</strong> {selectedCategory.id}
                                </p>
                                <p>
                                    <strong>Ad:</strong> {selectedCategory.name}
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

export default Categories;