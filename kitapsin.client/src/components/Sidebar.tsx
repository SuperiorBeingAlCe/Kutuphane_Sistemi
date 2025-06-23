import React, { useState } from 'react';
import {
  FaHome,
  FaBook,
  FaUser,
  FaBuilding,
  FaUsers,
  FaSignOutAlt,
  FaFileAlt,
    FaArchive,
    FaInfoCircle,
  FaTag,
} from 'react-icons/fa';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../components/AuthContext';

const Sidebar: React.FC = () => {
  const [isExpanded, setIsExpanded] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const { logout } = useAuth();

  const handleLogout = () => {
    logout();
    navigate('/login', { replace: true });
  };

  // Menü öğeleri
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

  const sidebarWidth = isExpanded ? 220 : 70;

  return (
    <div
      onMouseEnter={() => setIsExpanded(true)}
      onMouseLeave={() => setIsExpanded(false)}
      style={{
        width: sidebarWidth,
        height: '100vh',
        backgroundColor: '#1f1f1f',
        color: '#eee',
        display: 'flex',
        flexDirection: 'column',
        paddingTop: 20,
        position: 'fixed',
        top: 0,
        left: 0,
        transition: 'width 0.3s',
        boxShadow: '2px 0 5px rgba(0,0,0,0.5)',
        zIndex: 100,
      }}
    >
      {menuItems.map((item, index) => {
        const isActive = item.path === location.pathname;
        return (
          <div
            key={index}
            onClick={() => (item.path ? navigate(item.path) : item.action?.())}
            style={{
              display: 'flex',
              alignItems: 'center',
              padding: '12px 20px',
              cursor: 'pointer',
              color: isActive ? '#76c7c0' : '#ccc',
              backgroundColor: isActive ? '#333' : 'transparent',
              fontWeight: isActive ? '700' : '400',
              transition: 'background 0.2s, color 0.2s',
              userSelect: 'none',
            }}
            onMouseOver={(e) => {
              if (!isActive) e.currentTarget.style.background = '#333';
            }}
            onMouseOut={(e) => {
              if (!isActive) e.currentTarget.style.background = 'transparent';
            }}
          >
            <div
              style={{
                fontSize: 20,
                marginRight: isExpanded ? 15 : 0,
                minWidth: 20,
                display: 'flex',
                justifyContent: 'center',
              }}
            >
              {item.icon}
            </div>
            {isExpanded && <span style={{ whiteSpace: 'nowrap' }}>{item.label}</span>}
          </div>
        );
      })}
    </div>
  );
};

export default Sidebar;