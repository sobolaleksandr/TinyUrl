import { Box } from "@chakra-ui/react";
import TinyUrlForm from "../components/TinyUrlForm";
import Background from "../components/Background";

function Home() {
  return (
    <Box
      height="100%"
      display="flex"
      alignItems="center"
      justifyContent="center"
    >
      <TinyUrlForm />
      <Background />
    </Box>
  );
}

export default Home;