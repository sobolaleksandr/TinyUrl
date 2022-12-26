import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import HandleRedirectContainer from "./containers/HandleRedirect";
import HomeContainer from "./containers/Home";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<HomeContainer />} />
        <Route path="/:shortAddress" element={<HandleRedirectContainer />} />
      </Routes>
    </Router>
  );
}

export default App;