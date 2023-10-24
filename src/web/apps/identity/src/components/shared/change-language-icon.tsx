'use client';

import { useI18n } from '@sisa/i18n';

import ListItemButton from '@mui/joy/ListItemButton';
import { Dropdown, MenuButton, MenuItem } from '@mui/base';
import { Menu } from '@mui/joy';

const ChangeLanguageIcon = () => {
  const [t, i18n] = useI18n();
  const handleChangeLanguage = (lng: string) => {
    i18n.changeLanguage(lng);
  };

  return (
    <Dropdown open>
      <MenuButton
        slots={{
          root: ListItemButton,
        }}
      >
        {i18n.language}
      </MenuButton>
      <Menu keepMounted>
        <MenuItem onClick={() => handleChangeLanguage('vi')}>VI</MenuItem>
        {i18n.languages.map((lng) => (
          <MenuItem key={lng} onClick={() => handleChangeLanguage(lng)}>
            {
              // @ts-ignore
              t(`i18n.${lng}`)
            }
          </MenuItem>
        ))}
      </Menu>
    </Dropdown>
  );
};

export default ChangeLanguageIcon;
