'use client';

import Button from '@mui/joy/Button';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

import { ShieldQuestionIcon } from 'lucide-react';

import { FormActions, FormContainer, TextField, useForm, yup, yupResolver } from '@sisa/form';
import { Link } from '@sisa/next';

const LoginWith2faPage = () => {
  const validationSchema = yup.object({
    twoFaCode: yup.string().required().min(4).max(20).label('2FA Code'),
  });

  type FormValues = yup.InferType<typeof validationSchema>;

  const { control, handleSubmit } = useForm<FormValues>({
    defaultValues: {
      twoFaCode: '',
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
          Login with 2FA
        </Typography>
        <Typography level="body-sm">
          {`Please enter your 2FA code from your authenticator app.`}
        </Typography>
      </Stack>

      <FormContainer orientation="vertical">
        <TextField
          control={control}
          name="twoFaCode"
          label="2FA code"
          required
          placeholder="Enter your 2FA code"
          endDecorator={<Button variant="soft">Resend</Button>}
        />
        <FormActions display="flex" flex={1} mt={2}>
          <Button
            type="submit"
            variant="solid"
            color="primary"
            sx={{ flex: 1 }}
            onClick={onSubmit}
            startDecorator={<ShieldQuestionIcon />}
          >
            Verify
          </Button>
        </FormActions>
      </FormContainer>

      <Typography level="body-sm" color="neutral" textAlign="right" mt={2}>
        {'Have trouble with 2FA? '}
        <Link href="/login-with-recovery-code" color="primary" underline="always">
          {'Login with recovery code'}
        </Link>
      </Typography>
    </Stack>
  );
};

export default LoginWith2faPage;
