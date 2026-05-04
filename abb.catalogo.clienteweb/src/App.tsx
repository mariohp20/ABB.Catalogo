import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import MainLayout from './components/MainLayout';

// Componentes temporales (Placeholders) para probar la navegación
// En la siguiente fase, estos se separarán en archivos dentro de src/pages/
const Login = () => (
    <div className="d-flex justify-content-center align-items-center vh-100" style={{ backgroundColor: 'var(--abb-bg)' }}>
        <div className="abb-card text-center p-5">
            <h2 style={{ color: 'var(--abb-primary)', fontWeight: 'bold' }}>ABB Catálogo</h2>
            <p>Pantalla de Login en construcción...</p>
        </div>
    </div>
);

const Dashboard = () => (
    <div className="abb-card">
        <h3 className="fw-bold" style={{ color: 'var(--abb-primary)' }}>Panel Principal</h3>
        <hr />
        <p className="text-muted">Bienvenido al sistema de gestión. Seleccione una opción del menú lateral.</p>
    </div>
);

const Productos = () => (
    <div className="abb-card">
        <h3 style={{ color: 'var(--abb-text)' }}>Inventario de Motores y Transformadores</h3>
        <hr />
        <p>Aquí irá la tabla de datos y el CRUD de productos.</p>
    </div>
);

const Usuarios = () => (
    <div className="abb-card">
        <h3 style={{ color: 'var(--abb-text)' }}>Administración de Personal</h3>
        <hr />
        <p>Aquí irá la gestión de cuentas y roles del sistema.</p>
    </div>
);

function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    {/* Ruta Pública: No utiliza el MainLayout porque abarca toda la pantalla */}
                    <Route path="/login" element={<Login />} />

                    {/* Rutas Privadas: Envueltas en el Layout para heredar el Sidebar y Navbar */}
                    <Route path="/" element={<MainLayout />}>
                        {/* Si el usuario entra a la raíz "/", lo redirigimos al dashboard */}
                        <Route index element={<Navigate to="/dashboard" replace />} />

                        {/* Páginas inyectadas en el <Outlet /> del MainLayout */}
                        <Route path="dashboard" element={<Dashboard />} />
                        <Route path="productos" element={<Productos />} />
                        <Route path="usuarios" element={<Usuarios />} />
                    </Route>

                    {/* Manejo de error 404: Cualquier ruta no definida enviará al login */}
                    <Route path="*" element={<Navigate to="/login" replace />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}

export default App;