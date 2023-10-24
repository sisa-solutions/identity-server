const randomId = () => {
  const timestamp = Date.now().toString(36); // Convert current timestamp to base 36
  const randomPart = Math.random().toString(36).substr(2, 5); // Generate a random string and take 5 characters from it
  return timestamp + randomPart;
};

export default randomId;
