import { type ForwardedRef, forwardRef, type Ref, type ReactElement } from 'react';

import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import FormHelperText from '@mui/joy/FormHelperText';
import Select, { type SelectProps } from '@mui/joy/Select';
import Option from '@mui/joy/Option';

import { ChevronDownIcon } from 'lucide-react';

export type SelectOption<T extends string | number> = {
  value: T;
  label: string;
};

export type SelectionInputProps<
  T extends string | number,
  O extends SelectOption<T>,
> = SelectProps<O> & {
  label?: React.ReactNode;
  helperMessage?: React.ReactNode;
  errorMessage?: React.ReactNode;
  error?: boolean;
  loading?: boolean;
  options: O[];
};

const SelectionInput = forwardRef(
  <T extends string | number, O extends SelectOption<T>>(
    {
      label,
      helperMessage,
      errorMessage,
      required,
      error,
      disabled,
      loading,
      size,
      sx,
      options,
      ...inputProps
    }: SelectionInputProps<T, O>,
    ref: ForwardedRef<HTMLDivElement>
  ) => {
    return (
      <FormControl required={required} error={error} disabled={disabled} sx={sx} size={size}>
        <Box>
          {label && <FormLabel>{label}</FormLabel>}
          {helperMessage && <Typography level="body-sm">{helperMessage}</Typography>}
        </Box>
        <Box>
          <Select
            // clearIcon={<XIcon />}
            // popupIcon={<ChevronDownIcon />}
            slotProps={{
              root: {
                ref,
              },
            }}
            // loading={loading}
            indicator={<ChevronDownIcon />}
            {...inputProps}
          >
            {options.map(({ value, label }) => (
              <Option key={value} value={value}>
                {label}
              </Option>
            ))}
          </Select>
          {errorMessage && <FormHelperText>{errorMessage}</FormHelperText>}
        </Box>
      </FormControl>
    );
  }
);

SelectionInput.displayName = 'SelectionInput';

export default SelectionInput as <T extends string | number, O extends SelectOption<T>>(
  props: SelectionInputProps<T, O> & {
    ref?: Ref<HTMLDivElement>;
  }
) => ReactElement;
