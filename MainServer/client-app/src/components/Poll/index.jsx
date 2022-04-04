import { Alert, Button, Form, Spinner } from "react-bootstrap";
import { useState } from "react";
import dayjs from "dayjs";
import "./styles.css";

const Poll = () => {
  const [webServer, setWebServer] = useState("");
  const [isPolling, setIsPolling] = useState(false);
  const [isTimePolling, setIsTimePolling] = useState(false);
  const [polledTime, setPolledTime] = useState("");
  const [polledStatus, setPolledStatus] = useState(undefined);
  const [error, setError] = useState("");
  const [isStatusPolling, setIsStatusPolling] = useState(false);

  const pollTime = () => {
    setError("");
    setIsTimePolling(true);
    fetch(`/api/poll/time?from=${webServer}`)
      .then((response) => {
        if (response.ok) {
          return response.json();
        }
        throw new Error("Something went wrong. Please try again");
      })
      .then((correction) => {
        const time = dayjs(Math.round(correction / 1000)).format(
          "YYYY-MM-DD HH:mm:ss"
        );
        setPolledTime(time);
        setIsTimePolling(false);
      })
      .catch((error) => {
        setError(error.message);
        setIsTimePolling(false);
      });
  };

  const pollStatus = () => {
    setError("");
    setIsStatusPolling(true);

    fetch(`/api/poll/status?from=${webServer}`)
      .then((response) => {
        if (response.ok) {
          return response.json();
        }
        throw new Error("Something went wrong. Please try again");
      })
      .then((data) => {
        setIsStatusPolling(false);
        setPolledStatus(data);
      })
      .catch((error) => {
        setIsStatusPolling(false);
        setError(error.message);
      });
  };

  return (
    <Form className="polling-block" onSubmit={(e) => e.preventDefault()}>
      <Form.Group className="mb-3" controlId="formBasicEmail">
        <Form.Label>WebServer</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter ip"
          onChange={(e) => setWebServer(e.target.value)}
        />
      </Form.Group>
      <div className="polling-time">
        <span>
          <b>Time polled from server:</b> <br></br> {polledTime}
        </span>
        {isTimePolling && (
          <Spinner animation="border" variant="primary"></Spinner>
        )}
        <Button
          variant="primary"
          type="submit"
          onClick={pollTime}
          disabled={!webServer}
        >
          Poll Time
        </Button>
      </div>
      <div className="polling-time">
        <span>
          <b>Status polled from server:</b> <br></br>
          State: {polledStatus ? polledStatus.state : ""} <br></br>
          Served Requests: {polledStatus ? polledStatus.requestsServed : ""}
        </span>
        {isStatusPolling && (
          <Spinner animation="border" variant="primary"></Spinner>
        )}
        <Button
          variant="primary"
          type="submit"
          onClick={pollStatus}
          disabled={!webServer}
        >
          Poll Status
        </Button>
      </div>

      {error && <p className="alert">{error}</p>}
    </Form>
  );
};

export default Poll;
