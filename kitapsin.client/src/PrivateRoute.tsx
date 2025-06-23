import React from 'react';
import { useLocation, Navigate } from 'react-router-dom';
import { useAuth } from './components/AuthContext';

interface PrivateRouteProps {
    children: React.ReactNode;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ children }) => {
    const { isAuthenticated, isLoading } = useAuth();
    const location = useLocation();

    if (isLoading) {
        return <div style={{ color: 'white', textAlign: 'center', marginTop: '50px' }}>Yükleniyor...</div>;
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace state={{ from: location }} />;
    }

    return <>{children}</>;
};

export default PrivateRoute;