'use client';

import { observer } from 'mobx-react-lite';

import ListItem from '@mui/joy/ListItem';
import ListItemButton, { type ListItemButtonProps } from '@mui/joy/ListItemButton';

import { MenuIcon } from 'lucide-react';

import useStore from 'stores/use-store';

type Props = Omit<ListItemButtonProps, 'children'>;

const ToggleSidebarIcon = (props: Props) => {
  const sidebarStore = useStore('sidebarStore');

  return (
    <ListItem>
      <ListItemButton onClick={sidebarStore.openSidebar} {...props}>
        <MenuIcon />
      </ListItemButton>
    </ListItem>
  );
};

export default observer(ToggleSidebarIcon);
