import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaThLarge, FaThList } from 'react-icons/fa';
import type { JSX } from 'react/jsx-dev-runtime';

interface ShelfTypeOption {
    id: 'blocks' | 'rows';
    label: string;
    icon: JSX.Element;
    route: string;
}

const shelfOptions: ShelfTypeOption[] = [
    { id: 'blocks', label: 'A-Z Bloklar', icon: <FaThLarge size={32} />, route: '/shelf/blocks' },
    { id: 'rows', label: 'A-Z Raflar', icon: <FaThList size={32} />, route: '/shelf/rows' }
];

const ShelfSetup: React.FC = () => {
    const [layoutExists, setLayoutExists] = useState<boolean | null>(null);
    const [selectedOption, setSelectedOption] = useState<ShelfTypeOption | null>(null);
    const navigate = useNavigate();

    // Sayfa açıldığında tercihi kontrol et
    useEffect(() => {
        const checkLayout = async () => {
            const token = localStorage.getItem('token') ?? '';
            const adminId = parseInt(localStorage.getItem('adminId') ?? '0');
            if (!adminId) {
                setLayoutExists(false);
                return;
            }
            try {
                const res = await fetch(`https://localhost:7195/api/ShelfLayout/${adminId}`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (res.ok) {
                    const isBlock = (await res.json()) as boolean;
                    const opt = shelfOptions.find(o => (o.id === 'blocks') === isBlock) ?? null;
                    setSelectedOption(opt);
                    setLayoutExists(true);
                    navigate(opt!.route);
                } else if (res.status === 404) {
                    setLayoutExists(false);
                } else {
                    throw new Error();
                }
            } catch {
                setLayoutExists(false);
            }
        };
        checkLayout();
    }, [navigate]);

    // Seçim yapıldıktan sonra kaydet ve yönlendir
    const handleSave = async () => {
        if (!selectedOption) return;

        const isBlockLayout = selectedOption.id === 'blocks';
        const token = localStorage.getItem('token') ?? '';
        const adminId = parseInt(localStorage.getItem('adminId') ?? '0');
        if (!adminId) {
            alert('Admin ID bulunamadı. Giriş yapın.');
            return;
        }

        try {
            const res = await fetch(`https://localhost:7195/api/ShelfLayout/${adminId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify(isBlockLayout)
            });
            if (!res.ok) throw new Error('Tercih kaydedilemedi');
            navigate(selectedOption.route);
        } catch (err) {
            alert('Hata: ' + (err as Error).message);
        }
    };

    if (layoutExists === null) {
        return <div style={loadingStyle}>Yükleniyor...</div>;
    }

    if (layoutExists) {
        // Yönlendirme zaten useEffect içinde yapıldı
        return null;
    }

    return (
        <div style={containerStyle}>
            <div style={panelStyle}>
                <h2 style={titleStyle}>Raflığını Kur</h2>
                <p style={descStyle}>Raf türünü seç: A-Z bloklar veya A-Z raflar</p>
                <div style={optionsContainerStyle}>
                    {shelfOptions.map(option => (
                        <div
                            key={option.id}
                            style={{
                                ...optionCardStyle,
                                border:
                                    selectedOption?.id === option.id
                                        ? '2px solid #0b74de'
                                        : '2px solid transparent'
                            }}
                            onClick={() => setSelectedOption(option)}
                        >
                            {option.icon}
                            <span style={optionLabelStyle}>{option.label}</span>
                        </div>
                    ))}
                </div>
                <button
                    disabled={!selectedOption}
                    onClick={handleSave}
                    style={{
                        ...btnSaveStyle,
                        backgroundColor: selectedOption ? '#0b74de' : '#555',
                        cursor: selectedOption ? 'pointer' : 'not-allowed'
                    }}
                >
                    Kaydet ve Devam Et
                </button>
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
    boxSizing: 'border-box'
};
const panelStyle: React.CSSProperties = {
    width: '40vw',
    backgroundColor: '#1e1e1e',
    borderRadius: 8,
    boxShadow: '0 0 15px rgba(0,0,0,0.7)',
    padding: 30,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center'
};
const titleStyle: React.CSSProperties = {
    marginBottom: 8,
    fontSize: 24
};
const descStyle: React.CSSProperties = {
    marginBottom: 20,
    textAlign: 'center'
};
const optionsContainerStyle: React.CSSProperties = {
    display: 'flex',
    gap: 20,
    marginBottom: 30
};
const optionCardStyle: React.CSSProperties = {
    width: 120,
    height: 120,
    backgroundColor: '#2e2e2e',
    borderRadius: 8,
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    cursor: 'pointer',
    transition: 'border 0.2s'
};
const optionLabelStyle: React.CSSProperties = {
    marginTop: 8,
    fontSize: 14
};
const btnSaveStyle: React.CSSProperties = {
    width: '100%',
    padding: '12px 0',
    border: 'none',
    borderRadius: 5,
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold'
};
const loadingStyle: React.CSSProperties = {
    textAlign: 'center',
    fontSize: 18,
    color: '#fff'
};

export default ShelfSetup;
