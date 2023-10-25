'use client';

import Button from '@mui/joy/Button';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

import { KeyRoundIcon, MailIcon, RefreshCwIcon } from 'lucide-react';

import {
  FormActions,
  FormContainer,
  PasswordField,
  TextField,
  useForm,
  yup,
  yupResolver,
} from '@sisa/form';

const ResetPasswordPage = () => {
  const validationSchema = yup.object({
    email: yup.string().required().email().min(6).max(50).label('Email'),
    password: yup
      .string()
      .required()
      .min(8)
      .max(20)
      .label('Password')
      .test(
        'password',
        'Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.',
        (value) => {
          return /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])/.test(value);
        }
      ),
  });

  type FormValues = yup.InferType<typeof validationSchema>;

  const { control, handleSubmit } = useForm<FormValues>({
    defaultValues: {
      email: '',
    },
    resolver: yupResolver(validationSchema),
    reValidateMode: 'onBlur',
  });

  const onSubmit = handleSubmit((data: FormValues) => {
    console.log(data);
  });

  return (
    <Stack direction="column" gap={2}>
      <Stack
        direction="column"
        gap={1}
        sx={{
          mb: 2,
        }}
      >
        <Typography level="h3" color="primary">
          Reset password
        </Typography>
        <Typography level="body-sm">
          {`The password should be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number and one special character.`}
        </Typography>
      </Stack>

      <FormContainer orientation="vertical">
        <TextField
          control={control}
          name="email"
          label="Email"
          type="email"
          required
          placeholder="Enter your email address"
          startDecorator={<MailIcon />}
        />
        <PasswordField
          control={control}
          name="password"
          label="Password"
          required
          placeholder="Enter new password"
          startDecorator={<KeyRoundIcon />}
        />
        <FormActions display="flex" flex={1} mt={2}>
          <Button
            type="submit"
            variant="solid"
            color="primary"
            sx={{ flex: 1 }}
            onClick={onSubmit}
            startDecorator={<RefreshCwIcon />}
          >
            Reset password
          </Button>
        </FormActions>
      </FormContainer>
    </Stack>
  );
};

export default ResetPasswordPage;
