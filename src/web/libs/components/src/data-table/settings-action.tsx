'use client';

import { useMemo, useState, useCallback } from 'react';

import { RowData, Table, flexRender } from '@tanstack/react-table';

import Box from '@mui/joy/Box';
import Checkbox from '@mui/joy/Checkbox';
import IconButton from '@mui/joy/IconButton';

import Dropdown from '@mui/joy/Dropdown';
import ListItem from '@mui/joy/ListItem';
import ListItemContent from '@mui/joy/ListItemContent';
import Menu from '@mui/joy/Menu';
import MenuButton from '@mui/joy/MenuButton';

import Stack from '@mui/joy/Stack';

import {
  ArrowDownIcon,
  ArrowUpDownIcon,
  ArrowUpIcon,
  EyeIcon,
  EyeOffIcon,
  SettingsIcon,
} from 'lucide-react';

import { PREDEFINED_COLUMN_IDS } from './constants';

type Props<TData> = {
  table: Table<TData>;
};

const SettingsAction = <TData extends RowData>(props: Props<TData>) => {
  const { table } = props;

  const headers = useMemo(() => table.getLeafHeaders(), [table]);

  const [open, setOpen] = useState(false);

  const handleOpenChange = useCallback((_: React.SyntheticEvent | null, isOpen: boolean) => {
    setOpen(isOpen);
  }, []);

  return (
    <Dropdown open={open} onOpenChange={handleOpenChange}>
      <MenuButton
        slots={{
          root: IconButton,
        }}
      >
        <SettingsIcon />
      </MenuButton>
      <Menu keepMounted>
        {table
          .getState()
          .columnOrder.filter((columnId) => PREDEFINED_COLUMN_IDS.indexOf(columnId) === -1)
          .map((columnId) => {
            const column = table.getColumn(columnId);
            const header = headers.find((header) => header.column.id === columnId);

            if (!column || !header) {
              return null;
            }

            return (
              <ListItem
                key={columnId}
                sx={{
                  gap: 2,
                }}
              >
                <Stack
                  direction="row"
                  gap={1}
                  sx={{
                    '& > *': {
                      minWidth: 20,
                    },
                  }}
                >
                  <Box>
                    {column.getCanHide() && (
                      <Checkbox
                        variant="soft"
                        checkedIcon={<EyeIcon />}
                        uncheckedIcon={<EyeOffIcon />}
                        checked={column.getIsVisible()}
                        onClick={column.getToggleVisibilityHandler()}
                        sx={{
                          '--Icon-fontSize': '16px',
                        }}
                      />
                    )}
                  </Box>
                  <Box>
                    {column.getCanSort() && (
                      <IconButton
                        onClick={column.getToggleSortingHandler()}
                        size="sm"
                        variant="soft"
                        color="primary"
                        disabled={!column.getIsVisible()}
                        sx={{
                          '--IconButton-size': '22px',
                          ...(column.getIsSorted() === false && {
                            '&': {
                              opacity: 0.5,
                            },
                          }),
                        }}
                      >
                        {{
                          asc: <ArrowUpIcon />,
                          desc: <ArrowDownIcon />,
                        }[column.getIsSorted() as string] ?? <ArrowUpDownIcon />}
                      </IconButton>
                    )}
                  </Box>
                </Stack>
                <ListItemContent
                  sx={{
                    display: 'flex',
                  }}
                >
                  {flexRender(column.columnDef.header, header.getContext())}
                </ListItemContent>
              </ListItem>
            );
          })}
      </Menu>
    </Dropdown>
  );
};

export default SettingsAction;
