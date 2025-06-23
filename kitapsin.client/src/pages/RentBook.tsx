import React, { useState, useEffect, useCallback } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';

interface DtoLoanCreate {
    userId: number;
    bookId: number;
    bookTitle: string;
    dueDate: string;
}

interface BookOption {
    id: number;
    title: string;
}

interface Penalty {
    id: number;
    reason: string;
    amount: number;
    issuedAt: string;
    isPaid: boolean;
}

interface Book {
    id: number;
    title: string;
    authorName: string;
    categoryName: string;
    publisherName: string;
}

function debounceAsync(func: (arg: string) => Promise<void>, delay: number): (arg: string) => void {
    let timer: ReturnType<typeof setTimeout> | null = null;
    return (arg: string): void => {
        if (timer) clearTimeout(timer);
        timer = setTimeout(() => { func(arg).catch(console.error); }, delay);
    };
}

const RentBook: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [query, setQuery] = useState('');
    const [options, setOptions] = useState<BookOption[]>([]);
    const [selected, setSelected] = useState<BookOption | null>(null);
    const [loanDuration, setLoanDuration] = useState<number>(15);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);
    const [loadingData, setLoadingData] = useState(true);

    const [penalties, setPenalties] = useState<Penalty[]>([]);
    const [penaltyError, setPenaltyError] = useState<string | null>(null);

    const [borrowedBooks, setBorrowedBooks] = useState<Book[]>([]);
    const [borrowError, setBorrowError] = useState<string | null>(null);

    const navigate = useNavigate();
    const token = localStorage.getItem('token');

    const fetchOptions = useCallback(async (q: string) => {
        const url = q.trim()
            ? `https://localhost:7195/api/Book/search?title=${encodeURIComponent(q.trim())}`
            : 'https://localhost:7195/api/Book/all';

        const res = await fetch(url, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (!res.ok) throw new Error('Kitaplar alınamadı');
        const data: BookOption[] = await res.json();
        setOptions(data);
    }, [token]);

    const debouncedFetch = useCallback(debounceAsync(fetchOptions, 500), [fetchOptions]);

    useEffect(() => {
        if (query.trim()) debouncedFetch(query);
        else setOptions([]);
    }, [query, debouncedFetch]);

    const handleSelect = (opt: BookOption) => {
        setSelected(opt);
        setQuery(opt.title);
        setOptions([]);
    };

    // Kullanıcıya ait cezalar ve ödünç kitapları paralel çek,
    // biri hata verse bile diğeri gösterilmeye devam etsin
    useEffect(() => {
        if (!id || !token) {
            setLoadingData(false);
            return;
        }

        setLoadingData(true);
        setPenaltyError(null);
        setBorrowError(null);

        const headers = { Authorization: `Bearer ${token}` };
        const penaltyReq = axios.get<Penalty[]>(`https://localhost:7195/api/User/${id}/penalties`, { headers });
        const borrowReq = axios.get<Book[]>(`https://localhost:7195/api/User/${id}/borrowed-books`, { headers });

        Promise.allSettled([penaltyReq, borrowReq])
            .then(results => {
                const [penRes, borRes] = results;

                // Ceza sonuçları
                if (penRes.status === 'fulfilled') {
                    setPenalties(penRes.value.data || []);
                } else {
                    console.warn('Ceza fetch hatası:', penRes.reason);
                    setPenalties([]);
                    setPenaltyError('Ceza verisi alınamadı.');
                }

                // Ödünç kitap sonuçları
                if (borRes.status === 'fulfilled') {
                    setBorrowedBooks(borRes.value.data || []);
                } else {
                    console.warn('Ödünç kitap fetch hatası:', borRes.reason);
                    setBorrowedBooks([]);
                    setBorrowError('Ödünç kitap verisi alınamadı.');
                }
            })
            .finally(() => setLoadingData(false));
    }, [id, token]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        const parsedUserId = Number(id);
        if (!selected) return setError('Bir kitap seçin');
        if (selected.title !== query) return setError('Lütfen listeden bir kitap seçin');
        if (!parsedUserId || isNaN(parsedUserId)) return setError('Geçersiz kullanıcı bilgisi');
        if (!selected.id) return setError('Geçersiz kitap bilgisi');
        if (!token) return setError('Giriş yapmanız gerekiyor.');

        const now = new Date();
        now.setDate(now.getDate() + loanDuration);
        now.setHours(12, 0, 0, 0);
        const dto: DtoLoanCreate = {
            userId: parsedUserId,
            bookId: selected.id,
            bookTitle: selected.title,
            dueDate: now.toISOString(),
        };

        try {
            await axios.post('https://localhost:7195/api/Loan', dto, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });
            setSuccess(true);
            setTimeout(() => navigate(-1), 1500);
        } catch (err) {
            console.error('Kiralama hatası:', err);
            setError('Kiralanamadı');
            setSuccess(false);
        }
    };

    if (loadingData) {
        return (
            <div style={container}>
                <p style={{ color: '#fff' }}>Veriler yükleniyor...</p>
            </div>
        );
    }

    return (
        <div style={container}>
            <form onSubmit={handleSubmit} style={form}>
                <h2 style={title}>Kitap Kirala</h2>

                <label style={label}>
                    Kitap Ara
                    <input
                        style={input}
                        value={query}
                        onChange={e => { setQuery(e.target.value); debouncedFetch(e.target.value); }}
                        onFocus={() => debouncedFetch(query)}
                        placeholder="Kitap adı"
                        required
                    />
                    <ul style={dropdown}>
                        {options.map(opt => (
                            <li key={opt.id} style={item} onClick={() => handleSelect(opt)}>
                                {opt.title}
                            </li>
                        ))}
                    </ul>
                </label>

                <label style={label}>
                    Teslim Süresi
                    <select
                        style={input}
                        value={loanDuration}
                        onChange={e => setLoanDuration(Number(e.target.value))}
                        required
                    >
                        <option value={15}>15 gün</option>
                        <option value={30}>30 gün</option>
                    </select>
                </label>

                {error && <div style={errorStyle}>{error}</div>}
                {success && <div style={successStyle}>Kiralanma başarılı!</div>}

                <button type="submit" style={button}>Kirala</button>

                <hr style={{ marginTop: 20, borderColor: '#444' }} />

                <h3 style={{ color: '#fff' }}>Ceza Kayıtları</h3>
                {penaltyError && <p style={{ color: 'orange' }}>{penaltyError}</p>}
                {!penaltyError && penalties.length === 0 && <p style={{ color: '#888' }}>Ceza yok.</p>}
                {!penaltyError && penalties.length > 0 && (
                    <ul style={{ color: '#ddd' }}>
                        {penalties.map(p => (
                            <li key={p.id}>
                                {p.reason} – {p.amount}₺ – {p.isPaid ? 'Ödendi' : 'Ödenmedi'}
                            </li>
                        ))}
                    </ul>
                )}

                <h3 style={{ color: '#fff', marginTop: 20 }}>Ödünç Alınan Kitaplar</h3>
                {borrowError && <p style={{ color: 'orange' }}>{borrowError}</p>}
                {!borrowError && borrowedBooks.length === 0 && <p style={{ color: '#888' }}>Ödünç kitap yok.</p>}
                {!borrowError && borrowedBooks.length > 0 && (
                    <ul style={{ color: '#ccc' }}>
                        {borrowedBooks.map(b => (
                            <li key={b.id}>
                                {b.title} — {b.authorName} ({b.publisherName}, {b.categoryName})
                            </li>
                        ))}
                    </ul>
                )}
            </form>
        </div>
    );
};

// Stil objeleri
const container: React.CSSProperties = {
    width: '100vw',
    minHeight: '100vh',
    backgroundColor: '#121212',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20
};
const form: React.CSSProperties = {
    backgroundColor: '#1e1e1e',
    padding: 20,
    borderRadius: 8,
    boxShadow: '0 0 15px rgba(0,0,0,0.7)',
    width: 400,
    display: 'flex',
    flexDirection: 'column',
    gap: 15
};
const title: React.CSSProperties = { color: '#fff', textAlign: 'center' };
const label: React.CSSProperties = { display: 'flex', flexDirection: 'column', color: '#ccc' };
const input: React.CSSProperties = {
    marginTop: 5,
    padding: 10,
    borderRadius: 4,
    border: 'none',
    backgroundColor: '#2a2a2a',
    color: '#fff'
};
const dropdown: React.CSSProperties = {
    listStyle: 'none',
    padding: 0,
    marginTop: 5,
    maxHeight: 120,
    overflowY: 'auto',
    backgroundColor: '#2a2a2a',
    borderRadius: 4
};
const item: React.CSSProperties = {
    padding: 8,
    cursor: 'pointer',
    borderBottom: '1px solid #333',
    color: '#fff'
};
const button: React.CSSProperties = {
    marginTop: 10,
    padding: 12,
    backgroundColor: '#0b74de',
    color: '#fff',
    border: 'none',
    borderRadius: 5,
    cursor: 'pointer'
};
const errorStyle: React.CSSProperties = { color: '#e63946', textAlign: 'center' };
const successStyle: React.CSSProperties = { color: '#38a169', textAlign: 'center' };

export default RentBook;