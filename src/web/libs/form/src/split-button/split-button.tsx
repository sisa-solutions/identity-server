import { useRef, useState, type MouseEvent } from 'react';

import ButtonGroup, { type ButtonGroupProps } from '@mui/joy/ButtonGroup';
import Button from '@mui/joy/Button';
import IconButton from '@mui/joy/IconButton';

import Menu from '@mui/joy/Menu';
import MenuItem from '@mui/joy/MenuItem';

import ListItemDecorator from '@mui/joy/ListItemDecorator';

import { ChevronDownIcon, ChevronUpIcon } from 'lucide-react';

export type Props = ButtonGroupProps & {
  value?: string | number;
  options: Array<{
    value?: string | number;
    label: React.ReactNode;
    startDecorator?: React.ReactNode;
    endDecorator?: React.ReactNode;
    onClick?: (event: MouseEvent<HTMLButtonElement>) => void;
  }>;
};

const SplitButton = ({ options, value, ...rest }: Props) => {
  const actionRef = useRef<() => void | null>(null);
  const anchorRef = useRef<HTMLDivElement>(null);

  const [open, setOpen] = useState(false);
  const [selectedValue, setSelectedValue] = useState(value);

  const handleMenuItemClick = (_: MouseEvent<HTMLDivElement>, newValue: string | number) => {
    setSelectedValue(newValue);
    setOpen(false);
  };

  const selectedOption = options.find((option) => option.value === selectedValue);

  return (
    <>
      <ButtonGroup ref={anchorRef} {...rest}>
        <Button
          value={selectedValue}
          onClick={selectedOption?.onClick}
          startDecorator={selectedOption?.startDecorator}
          endDecorator={selectedOption?.endDecorator}
        >
          {selectedOption?.label}
        </Button>
        <IconButton
          size="sm"
          onMouseDown={() => {
            // @ts-ignore
            actionRef.current = () => setOpen(!open);
          }}
          onKeyDown={() => {
            // @ts-ignore
            actionRef.current = () => setOpen(!open);
          }}
          onClick={() => {
            actionRef.current?.();
          }}
        >
          <ChevronUpIcon display={open ? 'inherit' : 'none'} />
          <ChevronDownIcon display={!open ? 'inherit' : 'none'} />
        </IconButton>
      </ButtonGroup>
      <Menu open={open} onClose={() => setOpen(false)} anchorEl={anchorRef.current} keepMounted>
        {options.map((option) => (
          <MenuItem
            key={String(option.value!)}
            variant="plain"
            selected={option.value === selectedValue}
            onClick={(evt) => handleMenuItemClick(evt, option.value!)}
          >
            {option.startDecorator && (
              <ListItemDecorator>{option.startDecorator}</ListItemDecorator>
            )}
            {option.label}
            {option.endDecorator && <ListItemDecorator>{option.endDecorator}</ListItemDecorator>}
          </MenuItem>
        ))}
      </Menu>
    </>
  );
};

export default SplitButton;
