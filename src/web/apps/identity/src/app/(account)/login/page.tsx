'use client';

import { Button, Checkbox, Stack, Typography, Divider, ButtonGroup } from '@mui/joy';

import { FormActions, FormContainer, TextField, useForm } from '@sisa/form';
import { Link } from '@sisa/next';
import { GithubIcon, MailIcon, TwitterIcon } from 'lucide-react';

interface FormValues {
  username: string;
  password: string;
  rememberMe: boolean;
}

const LoginPage = () => {
  const { control, handleSubmit } = useForm<FormValues>({
    defaultValues: {
      username: '',
      password: '',
      rememberMe: false,
    },
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
        <Typography level="h2" color="primary">
          Login
        </Typography>
        <Divider
          sx={{
            '--Divider-childPosition': '0%',
            '--Divider-thickness': '1px',
            '--Divider-lineColor': 'var(--joy-palette-warning-outlinedBorder)',
            alignItems: 'baseline',
          }}
        >
          <Typography level="title-md">{`Let's get you logged in.`}</Typography>
        </Divider>
      </Stack>

      <FormContainer orientation="vertical">
        <TextField control={control} name="username" label="Username" required />
        <TextField
          control={control}
          helperMessage={
            <Link href="/forgot-password" color="primary" underline="always">
              Forgot password?
            </Link>
          }
          name="password"
          label="Password"
          required
          sx={{
            '& .form-label-group': {
              display: 'flex',
              justifyContent: 'space-between',
            },
          }}
        />
        <Checkbox label="Remember me" name="rememberMe" />
        <FormActions display="flex" flex={1} mt={2}>
          <Button type="submit" variant="solid" color="primary" sx={{ flex: 1 }} onClick={onSubmit}>
            Login
          </Button>
        </FormActions>
      </FormContainer>

      <Divider>or</Divider>

      <ButtonGroup
        orientation="horizontal"
        spacing={2}
        sx={{
          '& > button': {
            flex: 1,
          },
        }}
      >
        <Button>
          <MailIcon />
        </Button>
        <Button>
          <GithubIcon />
        </Button>
        <Button>
          <TwitterIcon />
        </Button>
      </ButtonGroup>

      <Typography level="body-md" textAlign="right" mt={2}>
        {`Don't have an account? `}
        <Link href="/register" color="primary" underline="always">
          Register here
        </Link>
      </Typography>
    </Stack>
  );
};

export default LoginPage;
