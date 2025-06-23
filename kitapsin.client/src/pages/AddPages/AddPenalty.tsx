import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';

interface Loan {
    id: number;
    bookTitle: string;
    dueDate: string;
}

const AddPenalty: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const userId = parseInt(id ?? '0');

    const [reason, setReason] = useState('');
    const [amount, setAmount] = useState<number>(0);
    const [loans, setLoans] = useState<Loan[]>([]);
    const [selectedLoanId, setSelectedLoanId] = useState<number | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<boolean>(false);

    const navigate = useNavigate();

    const formatDate = (dateStr: string): string => {
        const date = new Date(dateStr);
        return date.toLocaleDateString('tr-TR');
    };

    const handleFetchLoans = async () => {
        try {
            const token = localStorage.getItem('token');
            const { data } = await axios.get(`https://localhost:7195/api/User/${userId}/loans`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            const now = new Date();
            const overdueLoans = data.filter((loan: Loan) => new Date(loan.dueDate) < now);

            setLoans(overdueLoans);
            if (overdueLoans.length === 0) {
                setError('Bu kullanıcıya ait gecikmiş ödünç bulunmamaktadır. Ceza eklenemez.');
            } else {
                setError(null);
            }
        } catch {
            setError('Ödünçler getirilirken bir hata oluştu.');
        }
    };

    const handleReasonChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedReason = e.target.value;
        setReason(selectedReason);

        if (selectedReason === 'Kitap geç teslim edildi') {
            setAmount(20); // otomatik ceza
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!selectedLoanId) {
            setError('Lütfen geçerli bir ödünç seçin.');
            return;
        }

        try {
            const token = localStorage.getItem('token');
            await axios.post(
                'https://localhost:7195/api/Penalty',
                { userId, reason, amount },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                }
            );

            setSuccess(true);
            setError(null);
            setTimeout(() => navigate('/Users'), 1500);
        } catch {
            setSuccess(false);
            setError('Ceza eklenirken bir hata oluştu.');
        }
    };

    return (
        <div style={{
            width: '100vw',
            height: '100vh',
            backgroundColor: '#121212',
            color: '#fff',
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
        }}>
            <form
                onSubmit={handleSubmit}
                style={{
                    backgroundColor: '#1e1e1e',
                    padding: '20px',
                    borderRadius: '8px',
                    boxShadow: '0 0 15px rgba(0,0,0,0.7)',
                    width: '350px',
                }}
            >
                <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>Ceza Ekle</h2>

                <div style={{ marginBottom: '16px' }}>
                    <label htmlFor="userId">Kullanıcı ID</label>
                    <input
                        type="number"
                        id="userId"
                        value={userId}
                        disabled
                        style={{
                            width: '90%', padding: '8px', borderRadius: '4px',
                            border: '1px solid #555', backgroundColor: '#2a2a2a', color: '#fff',
                        }}
                    />
                    <button type="button" onClick={handleFetchLoans} style={{ marginTop: '8px' }}>
                        Gecikmiş Ödünçleri Getir
                    </button>
                </div>

                {loans.length > 0 && (
                    <div style={{ marginBottom: '16px' }}>
                        <label htmlFor="loan">Ödünç Seç</label>
                        <select
                            value={selectedLoanId ?? ''}
                            onChange={(e) => setSelectedLoanId(parseInt(e.target.value))}
                            style={{ width: '100%', padding: '8px', borderRadius: '4px', backgroundColor: '#2a2a2a', color: '#fff' }}
                        >
                            <option value="" disabled>-- Seçin --</option>
                            {loans.map((loan) => (
                                <option key={loan.id} value={loan.id}>
                                    {loan.bookTitle} - Bitiş: {formatDate(loan.dueDate)}
                                </option>
                            ))}
                        </select>
                    </div>
                )}

                <div style={{ marginBottom: '16px' }}>
                    <label htmlFor="reason">Sebep</label>
                    <select
                        id="reason"
                        value={reason}
                        onChange={handleReasonChange}
                        required
                        style={{
                            width: '100%', padding: '8px', borderRadius: '4px',
                            border: '1px solid #555', backgroundColor: '#2a2a2a', color: '#fff',
                        }}
                    >
                        <option value="" disabled>-- Sebep Seçin --</option>
                        <option value="Kitap geç teslim edildi">Kitap geç teslim edildi</option>
                        <option value="Diğer">Diğer</option>
                    </select>
                </div>

                <div style={{ marginBottom: '16px' }}>
                    <label htmlFor="amount">Tutar</label>
                    <input
                        type="number"
                        id="amount"
                        value={amount}
                        onChange={(e) => setAmount(parseFloat(e.target.value))}
                        required
                        style={{
                            width: '90%', padding: '8px', borderRadius: '4px',
                            border: '1px solid #555', backgroundColor: '#2a2a2a', color: '#fff',
                        }}
                    />
                </div>

                {error && <p style={{ color: 'red' }}>{error}</p>}
                {success && <p style={{ color: 'lightgreen' }}>Ceza başarıyla eklendi!</p>}

                <button
                    type="submit"
                    style={{
                        width: '100%', padding: '10px', backgroundColor: '#e53935',
                        border: 'none', color: '#fff', fontWeight: 'bold', borderRadius: '5px', cursor: 'pointer'
                    }}
                >
                    Kaydet
                </button>
            </form>
        </div>
    );
};

export default AddPenalty;
