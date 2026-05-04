import { CSidebar, CSidebarBrand, CSidebarNav, CNavItem } from '@coreui/react';
import CIcon from '@coreui/icons-react';
import { cilSpeedometer, cilStorage, cilPeople, cilAccountLogout } from '@coreui/icons';
import { useAuth } from '../context/AuthContext';
import { Link, useLocation, useNavigate } from 'react-router-dom';

interface AppSidebarProps {
    unfoldable: boolean;
    sidebarShow: boolean;
    setSidebarShow: (val: boolean) => void;
}

const AppSidebar = ({ unfoldable, sidebarShow, setSidebarShow }: AppSidebarProps) => {
    const { auth, logout } = useAuth();
    const location = useLocation();
    const navigate = useNavigate();

    const handleLogout = (e: React.MouseEvent) => {
        e.preventDefault();
        logout();
        navigate('/login');
    };

    const isActive = (path: string) => location.pathname === path;

    return (
        <CSidebar
            position="fixed"
            unfoldable={unfoldable}
            visible={sidebarShow}
            onVisibleChange={(visible) => setSidebarShow(visible)}
            style={{ backgroundColor: 'var(--abb-sidebar)', color: '#FFFFFF' }}
        >
            <CSidebarBrand className="d-none d-md-flex text-decoration-none" style={{ backgroundColor: '#0B1437' }}>
                <h4 className="m-3 text-white fw-bold" style={{ letterSpacing: '1px' }}>ABB DASH</h4>
            </CSidebarBrand>

            <CSidebarNav>
                <CNavItem as={Link} to="/dashboard" active={isActive('/dashboard')}>
                    <CIcon customClassName="nav-icon text-white" icon={cilSpeedometer} /> Panel Principal
                </CNavItem>

                <CNavItem as={Link} to="/productos" active={isActive('/productos')}>
                    <CIcon customClassName="nav-icon text-white" icon={cilStorage} /> Inventario
                </CNavItem>

                {auth?.rol === "1" && (
                    <CNavItem as={Link} to="/usuarios" active={isActive('/usuarios')}>
                        <CIcon customClassName="nav-icon text-white" icon={cilPeople} /> Usuarios
                    </CNavItem>
                )}

                {/* mt-auto empuja esto hasta el fondo. El color rgba le da un toque premium */}
                <div className="mt-auto mb-3 mx-2">
                    <CNavItem
                        href="#"
                        onClick={handleLogout}
                        style={{ backgroundColor: 'rgba(220, 53, 69, 0.1)', borderRadius: '8px' }}
                    >
                        <CIcon customClassName="nav-icon text-danger" icon={cilAccountLogout} />
                        <span className="text-danger fw-bold">Cerrar Sesión</span>
                    </CNavItem>
                </div>
            </CSidebarNav>
        </CSidebar>
    );
};

export default AppSidebar;