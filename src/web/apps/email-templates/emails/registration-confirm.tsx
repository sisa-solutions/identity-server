import {
  Body,
  Button,
  Container,
  Head,
  Heading,
  Html,
  Link,
  Preview,
  Section,
  Tailwind,
  Text,
} from '@react-email/components';
import * as React from 'react';

export const RegistrationConfirmEmail = () => {
  return (
    <Tailwind>
      <Html>
        <Head />
        <Body className="bg-white dark:bg-slate-800 my-auto mx-auto font-sans">
          <Preview>Welcome to {'@Model.UserName'} - please verify your email</Preview>
          <Container className="shadow-md bg-white dark:bg-slate-800 border-[#eaeaea] rounded-md my-[40px] mx-auto p-[16px] w-[400px]">
            <Text className="text-black text-[14px] leading-[24px]">
              Hello {'@Model.UserName'},
            </Text>
            <Heading className="text-black dark:text-white text-[24px] font-normal text-center p-0 my-[30px] mx-0">
              {`Thank you for signing up for @Model.Company.`}
            </Heading>
            <Text className="text-black dark:text-white text-[14px] leading-[24px]">
              {`To get started, please verify your email address by entering this code in our website: `}
              <code className="bg-gray-200 rounded px-[4px] py-[2px]">{'@Model.Code'}</code>
            </Text>
            <Text className="text-black dark:text-white text-[14px] leading-[24px]">
              {`Or, you can simply click on this link to activate your account: `}
            </Text>
            <Section className="text-center mt-[16px] mb-[16px]">
              <Button
                className="bg-sky-600 pt-[20px] py-[12px] px-[12px] rounded text-white text-[12px] font-semibold no-underline text-center"
                href={'@Model.Url'}
              >
                Verify your email
              </Button>
            </Section>
            <Text className="text-black dark:text-white text-[14px] leading-[24px]">
              {`If you have trouble clicking on the link, please copy and paste this URL into your browser: `}
              <Link href={'@Model.Url'} className="text-blue-600 no-underline">
                {'@Model.Url'}
              </Link>
            </Text>
            <Text className="text-black dark:text-white text-[14px] leading-[24px]">
              ----------------
              <br />
              {`Cheers,`}
              <br />
              {`The @Model.Company team`}
            </Text>
          </Container>
        </Body>
      </Html>
    </Tailwind>
  );
};

export default RegistrationConfirmEmail;
