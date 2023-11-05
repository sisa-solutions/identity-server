'use client';

import { type MouseEvent } from 'react';

import { observer } from 'mobx-react-lite';

import Box from '@mui/joy/Box';
import IconButton from '@mui/joy/IconButton';
import List from '@mui/joy/List';
import ListItem from '@mui/joy/ListItem';
import ListItemContent from '@mui/joy/ListItemContent';
import ListItemDecorator from '@mui/joy/ListItemDecorator';
import ListSubheader from '@mui/joy/ListSubheader';
import Sheet from '@mui/joy/Sheet';
import Typography from '@mui/joy/Typography';

import { LogOutIcon } from 'lucide-react';

import { Icon, LinkListItemButton } from '@sisa/components';

import useStore from 'stores/use-store';

const SidebarMenu = () => {
  const sidebarStore = useStore('sidebarStore');

  const clickMenuItemHandler = (
    event: MouseEvent<HTMLDivElement> & MouseEvent<HTMLAnchorElement>
  ) => {
    const code =
      event.currentTarget.getAttribute('data-sidebar-menu-item') ?? sidebarStore.selectedCode ?? '';

    sidebarStore.setSelectedItemCode(code);
  };

  return (
    <Sheet
      sx={{
        position: {
          xs: 'fixed',
          lg: 'sticky',
        },
        transform: {
          xs: 'translateX(calc(100% * (var(--sisa-sidebar-open, 0) - 1) + var(--sisa-sidebar-open, 0) * var(--sisa-sidebar-group-width, 0px)))',
          lg: 'none',
        },
        transition: 'transform 0.25s, width 0.25s',
        zIndex: 'var(--sisa-sidebar-content-zIndex)',
        top: 0,
        display: 'flex',
        flexShrink: 0,
        flexDirection: 'column',
        gap: 2,
        p: 2,
        width: 'var(--sisa-sidebar-content-width)',
        height: '100dvh',
        borderRight: '1px solid',
        borderColor: 'divider',
        ...(sidebarStore.items.length === 0 && {
          p: 0,
          width: 0,
          visibility: 'hidden',
        }),
      }}
    >
      <List
        sx={{
          gap: 1,
          '--ListItem-minHeight': '36px',
          '--ListItem-radius': '8px',
        }}
      >
        <ListSubheader role="presentation" sx={{ color: 'text.primary' }}>
          Dashboard
        </ListSubheader>

        {sidebarStore.allItems.map((item) => (
          <ListItem
            key={item.code}
            sx={{
              display: item.parentCode === sidebarStore.selectingGroupCode ? 'block' : 'none',
            }}
          >
            <LinkListItemButton
              href={item.path}
              data-sidebar-menu-item={item.code}
              {...(item.code === sidebarStore.selectedCode && {
                variant: 'soft',
                selected: true,
              })}
              onClick={clickMenuItemHandler}
            >
              <ListItemDecorator>
                <Icon name={item.icon} />
              </ListItemDecorator>
              <ListItemContent>{item.name}</ListItemContent>
            </LinkListItemButton>
          </ListItem>
        ))}
      </List>

      <Box sx={{ pl: 1, display: 'flex', alignItems: 'center' }}>
        <div>
          <Typography fontWeight="lg" level="body-md">
            Sang Au
          </Typography>
          <Typography level="body-sm">sang.au@outlook.com</Typography>
        </div>
        <IconButton variant="plain" sx={{ ml: 'auto' }}>
          <LogOutIcon />
        </IconButton>
      </Box>
    </Sheet>
  );
};

export default observer(SidebarMenu);
