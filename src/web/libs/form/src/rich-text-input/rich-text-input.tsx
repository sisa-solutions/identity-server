import { Suspense, forwardRef } from 'react';

import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import FormHelperText from '@mui/joy/FormHelperText';

import RichText, { type RichTextProps } from './rich-text';

export type RichTextInputProps = RichTextProps & {
  label?: React.ReactNode;
  helperMessage?: React.ReactNode;
  errorMessage?: React.ReactNode;
};

const RichTextInput = forwardRef<HTMLDivElement, RichTextInputProps>(
  ({ label, helperMessage, errorMessage, required, error, disabled, size, sx, ...inputProps }, ref) => {
    return (
      <FormControl required={required} error={error} disabled={disabled} sx={sx} size={size}>
        <Box>
          {label && <FormLabel>{label}</FormLabel>}
          {helperMessage && <Typography level="body-sm">{helperMessage}</Typography>}
        </Box>
        <Box>
          <Suspense>
            <RichText ref={ref} {...inputProps} />
          </Suspense>
          {errorMessage && <FormHelperText>{errorMessage}</FormHelperText>}
        </Box>
      </FormControl>
    );
  }
);

RichTextInput.displayName = 'RichTextInput';

export default RichTextInput;
