import React from 'react';
import { revealTrueDeveloper } from '../assets/Dxs';

const About: React.FC = () => {
    const message = revealTrueDeveloper();

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
                position: 'relative',
            }}
        >
            <h1 style={{ fontSize: 48, marginBottom: 40 }}>Kütüphane Takip Uygulaması</h1>
            <p style={{ fontSize: 20, maxWidth: 900, lineHeight: 1.8, color: '#ccc' }}>
                Geliştiricinin Github Adresi:{' '}
                <a
                    href="https://github.com/SuperiorBeingAlCe"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    https://github.com/SuperiorBeingAlCe
                </a>
                <br />
                <span style={{ color: '#76c7c0' }}>{message}</span>
            </p>

          
            <div
                style={{
                    position: 'fixed',
                    bottom: 10,
                    right: 10,
                    width: 20,
                    height: 20,
                    backgroundColor: 'transparent',
                    cursor: 'default',
                    zIndex: 9999,
                    userSelect: 'none',
                }}
                className="hiddenDeveloperBox"
            >
               
                <span
                    style={{
                        color: 'transparent',
                        fontSize: 12,
                        pointerEvents: 'none',
                        userSelect: 'none',
                    }}
                >
                    Geliştirici: Alperen Çelebi
                </span>

               
                <div
                    style={{
                        position: 'absolute',
                        bottom: '110%',
                        right: 0,
                        backgroundColor: '#222',
                        color: '#76c7c0',
                        padding: '10px 15px',
                        fontSize: 14,
                        borderRadius: 6,
                        opacity: 0,
                        pointerEvents: 'none',
                        transition: 'opacity 0.3s ease',
                        whiteSpace: 'nowrap',
                        userSelect: 'text',
                        boxShadow: '0 0 10px rgba(0,0,0,0.5)',
                        minWidth: 150,
                        textAlign: 'center',
                        zIndex: 10000,
                    }}
                    className="hiddenMessage"
                >
                    {message}
                </div>
            </div>

            <style>
                {`
          .hiddenDeveloperBox:hover .hiddenMessage {
            opacity: 1 !important;
            pointer-events: auto !important;
          }
        `}
            </style>
        </div>
    );
};

export default About;