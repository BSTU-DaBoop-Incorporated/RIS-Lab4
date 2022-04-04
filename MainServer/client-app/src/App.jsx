import { useState } from "react";
import { Alert, Button, Form } from "react-bootstrap";
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import Poll from "../src/components/Poll";

const App = () => {
  return (
    <div className="App">
      <Poll></Poll>
      <Poll></Poll>
    </div>
  );
};

export default App;
