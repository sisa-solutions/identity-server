'use client';

import Box from '@mui/joy/Box';

import useStore from 'stores/use-store';

const SidebarOverlay = () => {
  const sidebarStore = useStore('sidebarStore');

  return (
    <Box
      className="sidebar-overlay"
      sx={{
        position: 'fixed',
        zIndex: 'var(--sisa-sidebar-content-zIndex)',
        top: 0,
        left: 0,
        width: '100vw',
        height: '100vh',
        backgroundColor: 'background.backdrop',
        transition: 'opacity 0.25s',
        transform: {
          xs: 'translateX(calc(100% * (var(--sisa-sidebar-open, 0) - 1) + var(--sisa-sidebar-open, 0) * var(--sisa-sidebar-group-width, 0px)))',
          lg: 'translateX(-100%)',
        },
      }}
      onClick={sidebarStore.closeSidebar}
    />
  );
};

export default SidebarOverlay;
