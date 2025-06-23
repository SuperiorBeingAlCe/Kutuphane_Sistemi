
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

/**
 * Kullanıcı arayüzü
 */
interface User {
    id: number;
    fullName: string;
    email: string;
    phoneNumber: string;
    cardNumber: string;
    createdAt: string;
    expireAt?: string;
}

/**
 * Ödünç alınan kitap arayüzü
 */
interface Loan {
    id: number;
    bookTitle: string;
    loanDate: string;
    dueDate: string;
    returnDate?: string;
    isReturned: boolean;
}

/**
 * Ceza arayüzü
 */
interface Penalty {
    id: number;
    reason: string;
    userId: number;
    amount: number;
    issuedAt: string;
    isPaid: boolean;
}

/**
 * Kitap yanıt arayüzü
 */
interface BookResponse {
    id: number;
    title: string;
    authorName: string;
    categoryName: string;
    publisherName: string;
}

/**
 * Kullanıcılar sayfası
 */
const Users: React.FC = () => {
   
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const [selectedUser, setSelectedUser] = useState<User | null>(null);
    const [loans, setLoans] = useState<Loan[]>([]);
    const [penalties, setPenalties] = useState<Penalty[]>([]);
    const [borrowedBooks, setBorrowedBooks] = useState<BookResponse[]>([]);

    const [loanError, setLoanError] = useState<string | null>(null);
    const [penaltyError, setPenaltyError] = useState<string | null>(null);
    const [borrowError, setBorrowError] = useState<string | null>(null);

    const [detailsLoading, setDetailsLoading] = useState(false);

    const navigate = useNavigate();
    const totalUsers = users.length;
    // Kullanıcı listesi çekme
    useEffect(() => {
        (async () => {
            try {
                const token = localStorage.getItem('token');
                const res = await fetch('https://localhost:7195/api/User/all', {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (!res.ok) throw new Error('Kullanıcılar alınamadı');
                setUsers(await res.json());
            } catch (err) {
                setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
            } finally {
                setLoading(false);
            }
        })();
    }, []);

    // Seçilen kullanıcı detaylarını çekme
    useEffect(() => {
        if (!selectedUser) return;
        const fetchDetails = async () => {
            setDetailsLoading(true);
            setLoanError(null);
            setPenaltyError(null);
            setBorrowError(null);

            const token = localStorage.getItem('token');
            const headers = { Authorization: `Bearer ${token}` };
            const userId = selectedUser.id;

            const loanPromise = fetch(`https://localhost:7195/api/User/${userId}/loans`, { headers })
                .then(async res => {
                    if (!res.ok) {
                        const body = await res.text();
                        throw new Error(`Ödünçler hatası (${res.status}): ${body}`);
                    }
                    return res.json() as Promise<Loan[]>;
                });

            const penaltyPromise = fetch(`https://localhost:7195/api/User/${userId}/penalties`, { headers })
                .then(async res => {
                    if (!res.ok) {
                        const body = await res.text();
                        throw new Error(`Cezalar hatası (${res.status}): ${body}`);
                    }
                    return res.json() as Promise<Penalty[]>;
                });

            const borrowPromise = fetch(`https://localhost:7195/api/User/${userId}/borrowed-books`, { headers })
                .then(async res => {
                    if (!res.ok) {
                        const body = await res.text();
                        throw new Error(`Borçlu kitaplar hatası (${res.status}): ${body}`);
                    }
                    return res.json() as Promise<BookResponse[]>;
                });

            const [loanResult, penaltyResult, borrowResult] = await Promise.allSettled([
                loanPromise,
                penaltyPromise,
                borrowPromise
            ]);

            if (loanResult.status === 'fulfilled') setLoans(loanResult.value);
            else { console.warn(loanResult.reason); setLoans([]); setLoanError('Ödünç verisi alınamadı.'); }

            if (penaltyResult.status === 'fulfilled') setPenalties(penaltyResult.value);
            else { console.warn(penaltyResult.reason); setPenalties([]); setPenaltyError('Ceza verisi alınamadı.'); }

            if (borrowResult.status === 'fulfilled') setBorrowedBooks(borrowResult.value);
            else { console.warn(borrowResult.reason); setBorrowedBooks([]); setBorrowError('Borçlu kitap verisi alınamadı.'); }

            setDetailsLoading(false);
        };
        fetchDetails();
    }, [selectedUser]);

    // Kullanıcı silme
    /**
     * Kullanıcıyı siler
     * @param id Kullanıcı ID
     */
    const handleDelete = async (id: number) => {
        if (!window.confirm('Bu kullanıcıyı silmek istediğinize emin misiniz?')) return;
        try {
            const token = localStorage.getItem('token');
            const res = await fetch(`https://localhost:7195/api/User/${id}`, { method: 'DELETE', headers: { Authorization: `Bearer ${token}` } });
            if (!res.ok) throw new Error('Silme başarısız');
            setUsers(prev => prev.filter(u => u.id !== id));
            setSelectedUser(null);
        } catch (err) {
            alert(err instanceof Error ? err.message : 'Silme hatası');
        }
    };

    // Ödünç silme
    /**
     * Ödünç kaydını siler
     * @param loanId Ödünç ID
     */
    const handleDeleteLoan = async (loanId: number) => {
        try {
            const token = localStorage.getItem('token');
            const res = await fetch(`https://localhost:7195/api/Loan/${loanId}`, {
                method: 'DELETE',
                headers: { Authorization: `Bearer ${token}` }
            });
            if (!res.ok) throw new Error('Ödünç silinemedi');
            setLoans(prev => prev.filter(l => l.id !== loanId));
        } catch (err) {
            alert(err instanceof Error ? err.message : 'Ödünç silme hatası');
        }
    };

    // Ceza silme
    /**
     * Ceza kaydını siler
     * @param penaltyId Ceza ID
     */
    const handleDeletePenalty = async (penaltyId: number) => {
        try {
            const token = localStorage.getItem('token');
            const res = await fetch(`https://localhost:7195/api/Penalty/${penaltyId}`, {
                method: 'DELETE',
                headers: { Authorization: `Bearer ${token}` }
            });
            if (!res.ok) throw new Error('Ceza silinemedi');
            setPenalties(prev => prev.filter(p => p.id !== penaltyId));
        } catch (err) {
            alert(err instanceof Error ? err.message : 'Ceza silme hatası');
        }
    };

    // Yönlendirmeler
    /**
     * Kullanıcı güncelleme sayfasına yönlendirir
     * @param id Kullanıcı ID
     */
    const handleUpdate = (id: number) => navigate(`/Users/UpdateUser/${id}`);
    /**
     * Kitap kiralama sayfasına yönlendirir
     * @param id Kullanıcı ID
     */
    const handleRent = (id: number) => navigate(`/Loans/RentBook/${id}`);
    /**
     * Kullanıcı ekleme sayfasına yönlendirir
     */
    const handleAdd = () => navigate('/Users/AddUser');
    /**
     * Ceza ekleme sayfasına yönlendirir
     * @param id Kullanıcı ID
     */
    const handleAddPenalty = (id: number) => navigate(`/Users/${id}/Penalties/AddPenalty`);
    /**
     * Detay panelini kapatır
     */
    const closePanel = () => { setSelectedUser(null); setLoans([]); setPenalties([]); setBorrowedBooks([]); };

    if (loading) return <div style={containerStyle}>Yükleniyor...</div>;
    if (error) return <div style={containerStyle}>Hata: {error}</div>;

    const selectedId = selectedUser?.id;

    return (
        <div style={containerStyle}>
            <div style={listStyle}>
                <div style={{ textAlign: 'right', marginBottom: 16 }}>
                    <button onClick={handleAdd} style={btnAddStyle}>Kullanıcı Ekle</button>
                </div>
                <p style={{ margin: '10px 0', fontWeight: 'bold', color: '#fff' }}>
                    Toplam Kullanıcı Sayısı: {totalUsers}
                </p>

                <table style={tableStyle}>
                    <thead><tr><th style={thStyle}>#</th><th style={thStyle}>Ad Soyad</th></tr></thead>
                    <tbody>
                        {users.map((u, idx) => (
                            <tr
                                key={u.id}
                                onClick={() => setSelectedUser(selectedId === u.id ? null : u)}
                                style={{ backgroundColor: u.id === selectedId ? '#333' : 'transparent', cursor: 'pointer' }}
                            >
                                <td style={tdStyle}>{idx + 1}</td>
                                <td style={tdStyle}>{u.fullName}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            <div style={{ ...panelStyle, right: selectedUser ? 40 : '-45vw' }}>
                {selectedUser && (
                    <>
                        <button onClick={closePanel} style={closeBtnStyle}>&times;</button>
                        <h3>Kullanıcı Detayları</h3>
                        <p><strong>ID:</strong> {selectedUser.id}</p>
                        <p><strong>Ad Soyad:</strong> {selectedUser.fullName}</p>
                        <p><strong>Email:</strong> {selectedUser.email}</p>
                        <p><strong>Telefon:</strong> {selectedUser.phoneNumber}</p>
                        <p><strong>Kart No:</strong> {selectedUser.cardNumber}</p>
                        <p><strong>Eklenme:</strong> {new Date(selectedUser.createdAt).toLocaleDateString()}</p>
                        <p><strong>Geçerlilik:</strong> {selectedUser.expireAt ? new Date(selectedUser.expireAt).toLocaleDateString() : 'Süresiz'}</p>

                        {detailsLoading ? <p>Detay yükleniyor...</p> : (
                            <>
                                {/* Ödünçler */}
                                <h4>Ödünçler</h4>
                                {loanError && <p style={{ color: 'orange' }}>{loanError}</p>}
                                {!loanError && loans.length === 0 && <p>Ödünç alınmış kitap yok.</p>}
                                {!loanError && loans.length > 0 && (
                                    <ul style={subListStyle}>
                                        {loans.map(l => (
                                            <li key={l.id} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                                <span>{l.bookTitle} — {new Date(l.loanDate).toLocaleDateString()} → {new Date(l.dueDate).toLocaleDateString()}</span>
                                                <button onClick={() => handleDeleteLoan(l.id)} style={{ backgroundColor: '#c0392b', border: 'none', color: '#fff', padding: '4px 8px', borderRadius: 4, cursor: 'pointer' }}>Sil</button>
                                            </li>
                                        ))}
                                    </ul>
                                )}

                                {/* Borçlu Kitaplar */}
                                <h4>Borçlu Kitaplar</h4>
                                {borrowError && <p style={{ color: 'orange' }}>{borrowError}</p>}
                                {!borrowError && borrowedBooks.length === 0 && <p>Borçlu kitap yok.</p>}
                                {!borrowError && borrowedBooks.length > 0 && (
                                    <ul style={subListStyle}>
                                        {borrowedBooks.map(b => (
                                            <li key={b.id}>{b.title} — {b.authorName} ({b.categoryName}, {b.publisherName})</li>
                                        ))}
                                    </ul>
                                )}

                                {/* Cezalar */}
                                <h4>Cezalar</h4>
                                <button onClick={() => handleAddPenalty(selectedUser.id)} style={btnStyleUpdate}>Ceza Ekle</button>
                                {penaltyError && <p style={{ color: 'orange' }}>{penaltyError}</p>}
                                {!penaltyError && penalties.length === 0 && <p>Ceza kaydı yok.</p>}
                                {!penaltyError && penalties.length > 0 && (
                                    <ul style={subListStyle}>
                                        {penalties.map(p => (
                                            <li key={p.id} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                                <span>{p.reason} — {p.amount.toFixed(2)}₺ — {new Date(p.issuedAt).toLocaleDateString()}</span>
                                                <button onClick={() => handleDeletePenalty(p.id)} style={{ backgroundColor: '#c0392b', border: 'none', color: '#fff', padding: '4px 8px', borderRadius: 4, cursor: 'pointer' }}>Sil</button>
                                            </li>
                                        ))}
                                    </ul>
                                )}

                                <div style={{ marginTop: 'auto', display: 'flex', gap: 10 }}>
                                    <button onClick={() => handleDelete(selectedUser.id)} style={btnStyleDelete}>Sil</button>
                                    <button onClick={() => handleUpdate(selectedUser.id)} style={btnStyleUpdate}>Güncelle</button>
                                    <button onClick={() => handleRent(selectedUser.id)} style={btnStyleRent}>Kitap Kirala</button>
                                </div>
                            </>
                        )}
                    </>
                )}
            </div>
        </div>
    );
};

// Stil objeleri
const containerStyle: React.CSSProperties = { width: '100vw', height: '100vh', backgroundColor: '#121212', display: 'flex', alignItems: 'center', justifyContent: 'center', padding: 20 };
const listStyle: React.CSSProperties = { width: '45vw', maxHeight: '90vh', overflowY: 'auto', backgroundColor: '#1e1e1e', borderRadius: 8, boxShadow: '0 0 15px rgba(0,0,0,0.7)', padding: 20 };
const tableStyle: React.CSSProperties = { width: '100%', borderCollapse: 'collapse', color: '#fff' };
const thStyle: React.CSSProperties = { borderBottom: '2px solid #444', padding: 10, backgroundColor: '#2e2e2e' };
const tdStyle: React.CSSProperties = { padding: 8, borderBottom: '1px solid #333' };
const btnAddStyle: React.CSSProperties = { backgroundColor: '#0b74de', color: '#fff', padding: '8px 16px', border: 'none', borderRadius: 5, cursor: 'pointer', fontWeight: 'bold' };
const panelStyle: React.CSSProperties = { position: 'absolute', top: 20, width: '45vw', maxHeight: '90vh', backgroundColor: '#2a2a2a', borderRadius: 8, boxShadow: '0 0 20px rgba(0,0,0,0.9)', padding: 20, overflowY: 'auto', transition: 'right 0.3s ease', display: 'flex', flexDirection: 'column', zIndex: 99 };
const closeBtnStyle: React.CSSProperties = { alignSelf: 'flex-end', background: 'transparent', border: 'none', color: '#fff', fontSize: 24, cursor: 'pointer' };
const subListStyle: React.CSSProperties = { paddingLeft: 20, marginTop: 5, marginBottom: 10 };
const btnStyleDelete: React.CSSProperties = { backgroundColor: '#e63946', border: 'none', color: '#fff', padding: '8px 14px', borderRadius: 4, cursor: 'pointer' };
const btnStyleUpdate: React.CSSProperties = { backgroundColor: '#457b9d', border: 'none', color: '#fff', padding: '8px 14px', borderRadius: 4, cursor: 'pointer' };
const btnStyleRent: React.CSSProperties = { backgroundColor: '#38a169', border: 'none', color: '#fff', padding: '8px 14px', borderRadius: 4, cursor: 'pointer' };

export default Users;
