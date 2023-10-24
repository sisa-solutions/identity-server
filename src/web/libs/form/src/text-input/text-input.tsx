import { forwardRef } from 'react';

import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import FormHelperText from '@mui/joy/FormHelperText';
import Input, { type InputProps } from '@mui/joy/Input';

export type TextInputProps = InputProps & {
  label?: React.ReactNode;
  helperMessage?: React.ReactNode;
  errorMessage?: React.ReactNode;
};

const TextInput = forwardRef<HTMLDivElement, TextInputProps>(
  (
    { label, helperMessage, errorMessage, required, error, disabled, sx, size, ...inputProps },
    ref
  ) => {
    return (
      <FormControl required={required} error={error} disabled={disabled} sx={sx} size={size}>
        <Box>
          {label && <FormLabel>{label}</FormLabel>}
          {helperMessage && <Typography level="body-sm">{helperMessage}</Typography>}
        </Box>
        <Box>
          <Input ref={ref} {...inputProps} />
          {errorMessage && <FormHelperText>{errorMessage}</FormHelperText>}
        </Box>
      </FormControl>
    );
  }
);

TextInput.displayName = 'TextInput';

export default TextInput;
