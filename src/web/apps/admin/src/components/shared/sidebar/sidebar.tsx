import Box from '@mui/joy/Box';

import SidebarGroup from './sidebar-group';
import SidebarMenu from './sidebar-menu';
import SidebarOverlay from './sidebar-overlay';

const Sidebar = () => {
  return (
    <Box
      className="sisa-sidebar"
      sx={{
        display: 'flex',
        flexDirection: 'row',
        minHeight: '100dvh',
      }}
    >
      <SidebarGroup />
      <SidebarOverlay />
      <SidebarMenu />
    </Box>
  );
};

export default Sidebar;
