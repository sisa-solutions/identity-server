import { type ForwardedRef, forwardRef, type Ref, type ReactElement } from 'react';

import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import FormHelperText from '@mui/joy/FormHelperText';

import Autocomplete, { type AutocompleteProps } from '@mui/joy/Autocomplete';

import { ChevronDownIcon, LoaderIcon, XIcon } from 'lucide-react';

export type AutocompleteInputProps<
  T,
  Multiple extends boolean | undefined,
  DisableClearable extends boolean | undefined,
  FreeSolo extends boolean | undefined,
> = AutocompleteProps<T, Multiple, DisableClearable, FreeSolo> & {
  label?: React.ReactNode;
  helperMessage?: React.ReactNode;
  errorMessage?: React.ReactNode;
};

const AutocompleteInput = forwardRef(
  <
    T,
    Multiple extends boolean | undefined,
    DisableClearable extends boolean | undefined,
    FreeSolo extends boolean | undefined,
  >(
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
      ...inputProps
    }: AutocompleteInputProps<T, Multiple, DisableClearable, FreeSolo>,
    ref: ForwardedRef<HTMLDivElement>
  ) => {
    return (
      <FormControl required={required} error={error} disabled={disabled} sx={sx} size={size}>
        <Box>
          {label && <FormLabel>{label}</FormLabel>}
          {helperMessage && <Typography level="body-sm">{helperMessage}</Typography>}
        </Box>
        <Box>
          <Autocomplete
            required={required}
            clearIcon={<XIcon />}
            popupIcon={<ChevronDownIcon />}
            slotProps={{
              root: {
                ref,
              },
            }}
            loading={loading}
            endDecorator={loading ? <LoaderIcon className="animate-spin" /> : null}
            {...inputProps}
          />
          {errorMessage && <FormHelperText>{errorMessage}</FormHelperText>}
        </Box>
      </FormControl>
    );
  }
);

AutocompleteInput.displayName = 'AutocompleteInput';

export default AutocompleteInput as <
  T,
  Multiple extends boolean | undefined,
  DisableClearable extends boolean | undefined,
  FreeSolo extends boolean | undefined,
>(
  props: AutocompleteInputProps<T, Multiple, DisableClearable, FreeSolo> & {
    ref?: Ref<HTMLDivElement>;
  }
) => ReactElement;
