import { forwardRef, useState } from 'react';

import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import FormHelperText from '@mui/joy/FormHelperText';
import Input, { type InputProps } from '@mui/joy/Input';
import IconButton from '@mui/joy/IconButton';

import { EyeIcon, EyeOffIcon } from 'lucide-react';

export type PasswordInputProps = InputProps & {
  label?: React.ReactNode;
  helperMessage?: React.ReactNode;
  errorMessage?: React.ReactNode;
};

const PasswordInput = forwardRef<HTMLDivElement, PasswordInputProps>(
  (
    { label, helperMessage, errorMessage, required, error, disabled, sx, size, ...inputProps },
    ref
  ) => {
    const [showPassword, setShowPassword] = useState(false);
    const inputType = showPassword ? 'text' : 'password';
    const renderToggleIcon = () => (showPassword ? <EyeOffIcon /> : <EyeIcon />);

    const toggleShowPassword = () => setShowPassword((prev) => !prev);

    return (
      <FormControl
        className="form-control"
        required={required}
        error={error}
        disabled={disabled}
        sx={sx}
        size={size}
      >
        <Box className="form-label-group">
          {label && <FormLabel className="form-label">{label}</FormLabel>}
          {helperMessage && (
            <Typography level="body-sm" className="form-helper">
              {helperMessage}
            </Typography>
          )}
        </Box>
        <Box>
          <Input
            ref={ref}
            {...inputProps}
            type={inputType}
            endDecorator={
              <IconButton onClick={toggleShowPassword}>{renderToggleIcon()}</IconButton>
            }
            sx={{
              '& > input[type=password]': {
                '&::-ms-reveal,&::-ms-clear': {
                  display: 'none',
                },
                appearance: 'none',
              },
            }}
          />
          {errorMessage && <FormHelperText className="form-error">{errorMessage}</FormHelperText>}
        </Box>
      </FormControl>
    );
  }
);

PasswordInput.displayName = 'PasswordInput';

export default PasswordInput;
