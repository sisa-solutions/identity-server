'use client';

import { type MouseEvent } from 'react';

import { observer } from 'mobx-react-lite';

import Avatar from '@mui/joy/Avatar';
import Box from '@mui/joy/Box';
import Divider from '@mui/joy/Divider';
import List from '@mui/joy/List';
import ListItem from '@mui/joy/ListItem';
import Sheet from '@mui/joy/Sheet';

import { Icon, LinkListItemButton } from '@sisa/components';

import useStore from 'stores/use-store';

import ToggleColorSchemeListItemIcon from '../toggle-color-scheme-icon';

const SidebarGroup = () => {
  const sidebarStore = useStore('sidebarStore');

  const clickMenuGroupHandler = (
    event: MouseEvent<HTMLDivElement> & MouseEvent<HTMLAnchorElement>
  ) => {
    const groupCode =
      event.currentTarget.getAttribute('data-sidebar-menu-group') ??
      sidebarStore.selectedGroupCode ??
      '';

    sidebarStore.setSelectingGroupCode(groupCode);
  };

  return (
    <Sheet
      sx={{
        position: {
          xs: 'fixed',
          md: 'sticky',
        },
        transform: {
          xs: 'translateX(calc(100% * (var(--sisa-sidebar-open, 0) - 1)))',
          md: 'none',
        },
        transition: 'transform 0.25s',
        zIndex: 'var(--sisa-sidebar-group-zIndex)',
        top: 0,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        gap: 2,
        flexShrink: 0,
        p: 2,
        height: '100dvh',
        width: 'var(--sisa-sidebar-group-width)',
        borderRight: '1px solid',
        borderColor: 'divider',
      }}
    >
      <List
        sx={{
          gap: 2,
          '--ListItem-radius': '8px',
        }}
      >
        {sidebarStore.groups.map((group) => (
          <ListItem key={group.code}>
            <LinkListItemButton
              data-sidebar-menu-group={group.code}
              href={group.path ?? '#'}
              {...(group.code === sidebarStore.selectedGroupCode && {
                variant: 'solid',
                selected: true,
              })}
              {...(group.code === sidebarStore.selectingGroupCode && {
                variant: 'soft',
                selected: false,
              })}
              onClick={clickMenuGroupHandler}
            >
              <Icon name={group.icon} />
            </LinkListItemButton>
          </ListItem>
        ))}

        <Box sx={{ flex: 1 }} />
        <ToggleColorSchemeListItemIcon />
        {sidebarStore.systemGroups.map((group) => (
          <ListItem key={group.code}>
            <LinkListItemButton
              data-sidebar-menu-group={group.code}
              href={group.path ?? '#'}
              {...(group.code === sidebarStore.selectedGroupCode && {
                variant: 'solid',
                selected: true,
              })}
              {...(group.code === sidebarStore.selectingGroupCode && {
                variant: 'soft',
                selected: false,
              })}
              onClick={clickMenuGroupHandler}
            >
              <Icon name={group.icon} />
            </LinkListItemButton>
          </ListItem>
        ))}
      </List>
      <Divider />
      <Avatar size="lg">Sa</Avatar>
    </Sheet>
  );
};

export default observer(SidebarGroup);
