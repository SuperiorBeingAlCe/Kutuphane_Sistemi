import React from 'react';

const Home: React.FC = () => {
    
    return (

        <div
            style={{
                height: '100%',
                width: '100%',
                padding: 0,
                margin: 0,
                boxSizing: 'border-box',
                color: '#eee',
                overflowY: 'auto',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'center',
                textAlign: 'center',
                minHeight: '100vh',
            }}
        >
            <h1 style={{ fontSize: 48, marginBottom: 40 }}>Kütüphane Takip Uygulaması</h1>
            <p style={{ fontSize: 20, maxWidth: 900, lineHeight: 1.8, color: '#ccc' }}>
                Bu sistem, kütüphane verilerini yönetmek ve kullanıcılar için pratik bir kullanım sunmak amacıyla geliştirilmiştir.
                <br />
                <span style={{ color: '#76c7c0' }}>
                    React, TypeScript, .NET C# ve Xamp MySQL
                </span>{' '}
                teknolojileriyle inşa edilmiştir. Modern, güvenli ve ölçeklenebilir bir yapıya sahiptir.
            </p>
        </div>
    );
};

export default Home;