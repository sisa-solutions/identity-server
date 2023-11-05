'use client';

import Box from '@mui/joy/Box';
import List from '@mui/joy/List';
import ListItem from '@mui/joy/ListItem';
import Sheet from '@mui/joy/Sheet';

import { ToggleSidebarIcon } from './sidebar';
import ToggleColorSchemeListItemIcon from './toggle-color-scheme-icon';

const Header = () => {
  return (
    <Sheet
      variant="solid"
      color="primary"
      invertedColors
      sx={{
        display: { xs: 'flex', md: 'none' },
        gap: 1,
        alignItems: 'center',
        position: 'fixed',
        zIndex: 'var(--sisa-header-zIndex)',
        top: 0,
        px: 2,
        width: '100vw',
        height: {
          xs: 'var(--sisa-header-height)',
          md: '0',
        },
        boxShadow: 'sm',
      }}
    >
      <List
        orientation="horizontal"
        sx={{
          '& li > div': {
            borderRadius: 'xs',
          },
        }}
      >
        <ToggleSidebarIcon />
        <Box sx={{ flex: 1 }}></Box>
        <ToggleColorSchemeListItemIcon />
      </List>
    </Sheet>
  );
};

export default Header;
