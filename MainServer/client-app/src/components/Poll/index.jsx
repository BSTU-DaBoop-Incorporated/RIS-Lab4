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

  const [isDataPulling, setIsDataPulling] = useState(false);
  const [pulledData, setPulledData] = useState(undefined);

  const [isDataPushing, setIsDataPushing] = useState(false);
  const [pushedStatus, setPushedStatus] = useState("");

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

  const pullData = () => {
    setError("");
    setIsDataPulling(true);
    setPulledData(undefined);

    fetch(`/api/pull?from=${webServer}`).then(async (response) => {
      if (response.ok) {
        setPulledData(await response.json());
        setIsDataPulling(false);
        return;
      }

      setError(await response.text());
      setIsDataPulling(false);
    });
  };

  const pushData = () => {
    setError("");
    setIsDataPushing(true);
    setPushedStatus("");

    fetch(`/api/push?from=${webServer}`).then(async (response) => {
      setPushedStatus(await response.text());
      setIsDataPushing(false);
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
      <hr />
      <h2>Replications</h2>
      <div className="pull-data">
        <span>
          <b>Pulled data from server:</b>
          <br></br>
          {pulledData
            ? pulledData.map((data) => {
                return (
                  <div className="pulled-block">
                    <b>Key: </b>
                    {data.key} | <b>Value: </b> {data.value}
                  </div>
                );
              })
            : ""}{" "}
          <br></br>
        </span>
        {isDataPulling && (
          <Spinner animation="border" variant="primary"></Spinner>
        )}
        <Button
          variant="primary"
          type="submit"
          onClick={pullData}
          disabled={!webServer}
        >
          Pull Data
        </Button>
      </div>
      <div className="pull-data">
        <Button
          variant="primary"
          type="submit"
          onClick={pushData}
          disabled={!webServer}
        >
          Push Data to Service
        </Button>
        {isDataPushing && (
          <Spinner animation="border" variant="primary"></Spinner>
        )}
        {pushedStatus && <p className="ok">{pushedStatus}</p>}
      </div>
    </Form>
  );
};

export default Poll;
