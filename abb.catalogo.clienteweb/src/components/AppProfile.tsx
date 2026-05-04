import { useAuth } from '../context/AuthContext';

const AppProfile = () => {
    const { auth } = useAuth();

    const obtenerNombreRol = (idRol: string | undefined) => {
        switch (idRol) {
            case '1': return 'Administrador';
            case '2': return 'Vendedor';
            case '3': return 'Almacenero';
            default: return 'Empleado';
        }
    };

    return (
        <div className="d-flex align-items-center gap-3">
            <div className="text-end d-none d-md-block">
                <h6 className="mb-0 fw-bold" style={{ color: 'var(--abb-text)' }}>{auth?.nombres || 'Cargando...'}</h6>
                <small className="text-muted">{obtenerNombreRol(auth?.rol)}</small>
            </div>

            <div className="rounded-circle d-flex justify-content-center align-items-center text-white shadow-sm"
                style={{ width: '42px', height: '42px', backgroundColor: 'var(--abb-primary)', fontWeight: 'bold' }}>
                {auth?.nombres ? auth.nombres.charAt(0).toUpperCase() : 'U'}
            </div>
        </div>
    );
};

export default AppProfile;