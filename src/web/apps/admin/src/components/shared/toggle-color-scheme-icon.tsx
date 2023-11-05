'use client';

import { useEffect, useState } from 'react';

import ListItem from '@mui/joy/ListItem';
import ListItemButton, { type ListItemButtonProps } from '@mui/joy/ListItemButton';

import { useColorScheme } from '@mui/joy/styles';

import { MoonIcon, SunIcon, SunMoonIcon } from 'lucide-react';

const ToggleColorSchemeListItemIcon = (props: ListItemButtonProps) => {
  const [mounted, setMounted] = useState(false);
  const { mode, setMode } = useColorScheme();

  // necessary for server-side rendering
  // because mode is undefined on the server
  useEffect(() => {
    setMounted(true);
  }, []);

  if (!mounted) {
    return (
      <ListItem>
        <ListItemButton {...props} onClick={(evt) => evt.preventDefault()}>
          <SunMoonIcon />
        </ListItemButton>
      </ListItem>
    );
  }

  const toggleModeHandler = () => {
    setMode(mode === 'light' ? 'dark' : 'light');
  };

  return (
    <ListItem>
      <ListItemButton onClick={toggleModeHandler} {...props}>
        <SunIcon display={mode === 'light' ? 'block' : 'none'} />
        <MoonIcon display={mode === 'dark' ? 'block' : 'none'} />
      </ListItemButton>
    </ListItem>
  );
};

export default ToggleColorSchemeListItemIcon;
