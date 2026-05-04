/* eslint-disable react-refresh/only-export-components */
import { createContext, useState, useContext, type ReactNode } from 'react';
import { jwtDecode } from 'jwt-decode';

// 1. Interfaz para el Estado de Autenticación
interface AuthState {
    token: string;
    idUsuario: number;
    nombres: string;
    rol: string;
}

// 2. Interfaz para los datos exactos que vienen dentro de nuestro JWT de C#
interface CustomJwtPayload {
    IdUsuario: number;
    Nombres: string;
    role: string;
}

// 3. Interfaz para el Contexto
interface AuthContextType {
    auth: AuthState | null;
    login: (token: string) => void;
    logout: () => void;
}

// Creamos el contexto (No lo exportamos para no molestar al Fast Refresh de Vite)
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// 4. Proveedor del Contexto
export const AuthProvider = ({ children }: { children: ReactNode }) => {

    // Inicialización "Perezosa" (Lazy Initialization). 
    // Leemos el localStorage directamente aquí para evitar el error de useEffect.
    const [auth, setAuth] = useState<AuthState | null>(() => {
        const token = localStorage.getItem('token');
        if (token) {
            try {
                const decodificado = jwtDecode<CustomJwtPayload>(token);
                return {
                    token: token,
                    idUsuario: decodificado.IdUsuario,
                    nombres: decodificado.Nombres,
                    rol: decodificado.role
                };
            } catch (error) {
                console.error("Error al decodificar el token", error);
                localStorage.removeItem('token');
                return null;
            }
        }
        return null;
    });

    const login = (token: string) => {
        localStorage.setItem('token', token);
        const decodificado = jwtDecode<CustomJwtPayload>(token);
        setAuth({
            token: token,
            idUsuario: decodificado.IdUsuario,
            nombres: decodificado.Nombres,
            rol: decodificado.role
        });
    };

    const logout = () => {
        localStorage.removeItem('token');
        setAuth(null);
    };

    return (
        <AuthContext.Provider value={{ auth, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

// 5. Custom Hook para el uso del contexto
export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth debe usarse dentro de un AuthProvider");
    }
    return context;
};