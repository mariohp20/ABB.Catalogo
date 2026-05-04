import { useState } from 'react';
import { Outlet } from 'react-router-dom';
import { CHeader, CContainer, CHeaderToggler, CHeaderBrand } from '@coreui/react';
import CIcon from '@coreui/icons-react';
import { cilMenu } from '@coreui/icons';

import AppSidebar from './AppSidebar';
import AppProfile from './AppProfile';

const MainLayout = () => {
    const [sidebarShow, setSidebarShow] = useState(true);
    const [unfoldable, setUnfoldable] = useState(false);

    return (
        <div>
            <AppSidebar
                unfoldable={unfoldable}
                sidebarShow={sidebarShow}
                setSidebarShow={setSidebarShow}
            />

            {/* AQUÍ ESTÁ LA MAGIA: Le inyectamos la clase 'sidebar-narrow' si está encogido */}
            <div className={`wrapper d-flex flex-column min-vh-100 bg-light ${unfoldable ? 'sidebar-narrow' : ''} ${!sidebarShow ? 'sidebar-hidden' : ''}`}>

                <CHeader position="sticky" className="mb-4 p-0 header border-0" style={{ backgroundColor: '#FFFFFF', minHeight: '70px' }}>
                    <CContainer fluid className="px-4">

                        {/* Botón PC */}
                        <button
                            className="border-0 bg-transparent text-dark d-none d-md-block ps-1"
                            onClick={() => setUnfoldable(!unfoldable)}
                            style={{ cursor: 'pointer' }}
                        >
                            <CIcon icon={cilMenu} size="lg" />
                        </button>

                        {/* Botón Celular */}
                        <CHeaderToggler
                            className="ps-1 d-md-none"
                            onClick={() => setSidebarShow(!sidebarShow)}
                        >
                            <CIcon icon={cilMenu} size="lg" />
                        </CHeaderToggler>

                        <CHeaderBrand className="mx-auto d-md-none fw-bold" style={{ color: 'var(--abb-text)' }}>
                            ABB DASH
                        </CHeaderBrand>

                        <div className="ms-auto">
                            <AppProfile />
                        </div>
                    </CContainer>
                </CHeader>

                <div className="body flex-grow-1 px-4 mt-2">
                    <CContainer fluid className="p-0">
                        <Outlet />
                    </CContainer>
                </div>
            </div>
        </div>
    );
};

export default MainLayout;