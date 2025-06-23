import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

interface Author {
  id: number;
  name: string;
}

const UpdateAuthor: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [author, setAuthor] = useState<Author>({
    id: Number(id),
    name: '',
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAuthor = async () => {
      try {
        const token = localStorage.getItem('token');
        const res = await fetch(`https://localhost:7195/api/Author/${id}`, {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        });

        if (!res.ok) throw new Error('Yazar verisi alınamadı.');

        const data = await res.json();
        setAuthor(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Bilinmeyen hata');
      } finally {
        setLoading(false);
      }
    };

    if (id) fetchAuthor();
  }, [id]);

  const handleUpdate = async () => {
    try {
      const token = localStorage.getItem('token');
      const res = await fetch(`https://localhost:7195/api/Author/${id}`, {
        method: 'PUT',
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(author),
      });

      if (!res.ok) throw new Error('Yazar güncelleme başarısız.');

      alert('Yazar başarıyla güncellendi.');
      navigate('/authors');
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Bilinmeyen hata');
    }
  };

  if (loading) return <div style={loadingStyle}>Yükleniyor...</div>;
  if (error) return <div style={errorStyle}>Hata: {error}</div>;

  return (
    <div style={pageStyle}>
      <div style={formStyle}>
        <h2>Yazar Güncelle</h2>

        <label style={labelStyle}>
          ID:
          <input
            type="number"
            value={author.id}
            disabled
            style={inputStyle}
          />
        </label>

        <label style={labelStyle}>
          İsim:
          <input
            type="text"
            value={author.name}
            onChange={(e) => setAuthor({ ...author, name: e.target.value })}
            style={inputStyle}
          />
        </label>

        <button onClick={handleUpdate} style={buttonStyle}>
          Güncelle
        </button>
      </div>
    </div>
  );
};

// Stil sabitleri — aynı şekilde kalabilir
const pageStyle: React.CSSProperties = {
  height: '100vh',
  backgroundColor: '#121212',
  color: '#fff',
  display: 'flex',
  justifyContent: 'center',
  alignItems: 'center',
};

const formStyle: React.CSSProperties = {
  backgroundColor: '#1e1e1e',
  padding: '30px',
  borderRadius: '10px',
  width: '400px',
  display: 'flex',
  flexDirection: 'column',
  gap: '15px',
  boxShadow: '0 0 10px rgba(0,0,0,0.7)',
};

const labelStyle: React.CSSProperties = {
  fontSize: '14px',
  fontWeight: 'bold',
};

const inputStyle: React.CSSProperties = {
  padding: '10px',
  marginTop: '5px',
  borderRadius: '5px',
  border: 'none',
  width: '100%',
  backgroundColor: '#2a2a2a',
  color: '#fff',
};

const buttonStyle: React.CSSProperties = {
  padding: '10px',
  backgroundColor: '#0b74de',
  color: 'white',
  border: 'none',
  borderRadius: '5px',
  fontWeight: 'bold',
  cursor: 'pointer',
};

const loadingStyle: React.CSSProperties = {
  color: '#ccc',
  fontSize: '20px',
  textAlign: 'center',
  marginTop: '40px',
};

const errorStyle: React.CSSProperties = {
  color: 'red',
  fontSize: '18px',
  textAlign: 'center',
  marginTop: '40px',
};

export default UpdateAuthor;