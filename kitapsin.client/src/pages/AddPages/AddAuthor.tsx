import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const AddAuthor: React.FC = () => {
  const [Name, setName] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);

  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const token = localStorage.getItem('token');
      const response = await axios.post(
        'https://localhost:7195/api/Author',
        { Name },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        }
      );

      console.log('Yazar başarıyla oluşturuldu:', response.data);
      setSuccess(true);
      setError(null);

      setTimeout(() => {
        navigate('/Authors'); 
      }, 1500);
    } catch (err) {
      setSuccess(false);
      if (axios.isAxiosError(err) && err.response && err.response.data) {
        setError(
          (err.response.data as { message?: string }).message ||
            'Yazar eklenemedi.'
        );
      } else {
        setError('Sunucuya bağlanılamadı.');
      }
    }
  };

  return (
    <div
      style={{
        width: '100vw',
        height: '100vh',
        backgroundColor: '#121212',
        color: '#fff',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
      }}
    >
      <form
        onSubmit={handleSubmit}
        style={{
          backgroundColor: '#1e1e1e',
          padding: '20px',
          borderRadius: '8px',
          boxShadow: '0 0 15px rgba(0,0,0,0.7)',
          width: '300px',
        }}
      >
        <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>
          Yazar Ekle
        </h2>

        <div style={{ marginBottom: '16px' }}>
          <label
            htmlFor="name"
            style={{ display: 'block', marginBottom: '8px' }}
          >
            Yazar Adı
          </label>
          <input
            type="text"
            id="name"
            value={Name}
            onChange={(e) => setName(e.target.value)}
            required
            style={{
              width: '90%',
              padding: '8px',
              borderRadius: '4px',
              border: '1px solid #555',
              backgroundColor: '#2a2a2a',
              color: '#fff',
            }}
          />
        </div>

        {error && <p style={{ color: 'red' }}>{error}</p>}
        {success && <p style={{ color: 'lightgreen' }}>Yazar başarıyla eklendi!</p>}

        <button
          type="submit"
          style={{
            width: '100%',
            padding: '10px',
            backgroundColor: '#0b74de',
            border: 'none',
            color: '#fff',
            fontWeight: 'bold',
            borderRadius: '5px',
            cursor: 'pointer',
          }}
        >
          Kaydet
        </button>
      </form>
    </div>
  );
};

export default AddAuthor;