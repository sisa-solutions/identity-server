import Link from 'next/link';

const NotFoundPage = () => {
  return (
    <div>
      <h2>Not Found</h2>
      <p>Could not find requested resource</p>
      <p>
        Go to <Link href="/">Home Page</Link>
      </p>
    </div>
  );
};

export default NotFoundPage;
