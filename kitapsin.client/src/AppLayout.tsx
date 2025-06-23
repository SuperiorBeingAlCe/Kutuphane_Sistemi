import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import {
    FaBook, FaUser, FaBuilding, FaUsers,
    FaSignOutAlt, FaFileAlt, FaArchive, FaTag,
    FaHome, FaInfoCircle, 
} from 'react-icons/fa';
import { useAuth } from './components/AuthContext';

const AppLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [isExpanded, setIsExpanded] = useState(false);
    const navigate = useNavigate();
    const location = useLocation();
    const { logout } = useAuth();

    const handleLogout = () => {
        logout();
        navigate('/login', { replace: true });
    };

    const sidebarWidth = isExpanded ? 220 : 70;

    const menuItems = [
        { icon: <FaHome />, label: 'Ana Sayfa', path: '/' },
        { icon: <FaBook />, label: 'Kitaplar', path: '/Books' },
        { icon: <FaUser />, label: 'Yazarlar', path: '/Authors' },
        { icon: <FaBuilding />, label: 'Yayımcılar', path: '/publishers' },
        { icon: <FaUsers />, label: 'Kullanıcılar', path: '/Users' },
        { icon: <FaTag />, label: 'Kategoriler', path: '/Categories' },
        { icon: <FaArchive />, label: 'Raflığım' },
        { icon: <FaFileAlt />, label: 'API Dökümantasyonu', path: '/docs' },
        { icon: <FaInfoCircle />, label: 'Hakkında', path: '/About' }, 
        { icon: <FaSignOutAlt />, label: 'Çıkış Yap', action: handleLogout },
    ];

    return (
        <div
            style={{
                display: 'flex',
                width: '100vw',
                height: '100vh',
                backgroundColor: '#121212',
                overflow: 'hidden',
            }}
        >
            {/* Sidebar */}
            <nav
                onMouseEnter={() => setIsExpanded(true)}
                onMouseLeave={() => setIsExpanded(false)}
                style={{
                    width: sidebarWidth,
                    height: '100%',
                    backgroundColor: '#1f1f1f',
                    color: '#eee',
                    display: 'flex',
                    flexDirection: 'column',
                    paddingTop: 20,
                    transition: 'width 0.3s',
                    position: 'fixed',
                    top: 0,
                    left: 0,
                    zIndex: 10,
                }}
            >
                {menuItems.map((item, idx) => {
                    const isActive = location.pathname === item.path;
                    return (
                        <div
                            key={idx}
                            onClick={() => (item.path ? navigate(item.path) : item.action?.())}
                            style={{
                                display: 'flex',
                                alignItems: 'center',
                                padding: '12px 20px',
                                cursor: 'pointer',
                                color: isActive ? '#76c7c0' : '#ccc',
                                backgroundColor: isActive ? '#333' : 'transparent',
                                fontWeight: isActive ? 'bold' : 'normal',
                                transition: 'background 0.2s, color 0.2s',
                            }}
                            onMouseOver={(e) => !isActive && (e.currentTarget.style.background = '#333')}
                            onMouseOut={(e) => !isActive && (e.currentTarget.style.background = 'transparent')}
                        >
                            <div style={{ fontSize: 20, marginRight: isExpanded ? 15 : 0 }}>
                                {item.icon}
                            </div>
                            {isExpanded && <span style={{ whiteSpace: 'nowrap' }}>{item.label}</span>}
                        </div>
                    );
                })}
            </nav>

            {/* Content Area */}
            <main
                style={{
                    marginLeft: sidebarWidth,
                    width: `calc(100vw - ${sidebarWidth}px)`,
                    height: '100vh',
                    overflowY: 'auto',
                    boxSizing: 'border-box',
                    color: '#eee',
                    padding: 0,  
                    margin: 0,
                    display: 'flex',
                    flexDirection: 'column',
                }}
            >
                {children}
            </main>
        </div>
    );
};

export default AppLayout;