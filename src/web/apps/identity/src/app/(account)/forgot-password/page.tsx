'use client';

import Button from '@mui/joy/Button';
import Card from '@mui/joy/Card';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

import { MailIcon } from 'lucide-react';

import { FormActions, FormContainer, TextField, useForm, yup, yupResolver } from '@sisa/form';
import { Link } from '@sisa/next';

const ForgotPasswordPage = () => {
  const validationSchema = yup.object({
    email: yup.string().required().email().min(6).max(50).label('Email'),
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
          Forgot password
        </Typography>
        <Card variant="soft">
          <Typography level="body-sm">
            {`Enter your email address and we'll send you a link to reset your password.`}
          </Typography>
        </Card>
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
        <FormActions display="flex" flex={1} mt={2}>
          <Button
            type="submit"
            variant="solid"
            color="primary"
            sx={{ flex: 1 }}
            onClick={onSubmit}
            startDecorator={<MailIcon />}
          >
            Send password reset email
          </Button>
        </FormActions>
      </FormContainer>

      <Typography level="body-sm" color="neutral" textAlign="right" mt={2}>
        {`I have an account! `}
        <Link href="/login" color="primary" underline="always">
          Login
        </Link>
      </Typography>
    </Stack>
  );
};

export default ForgotPasswordPage;
