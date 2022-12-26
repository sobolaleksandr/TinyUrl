import axios from "axios";
import { useEffect, useState } from "react";
import { useMatch } from "react-router-dom";
import { Spinner, Box } from "@chakra-ui/react";
import { SERVER_ENDPOINTS } from "../config";

function HandleRedirectContainer() {
    const [destination, setDestination] = useState<null | string>(null);
    const [error, setError] = useState();
    const { params: { shortAddress } } = useMatch({ path: "shortAddress", end: false }) ?? { params: { shortAddress: "" } };

    useEffect(() => {
        async function getData() {
            return axios
                .get(`${SERVER_ENDPOINTS}?shortUrl=${shortAddress}`)
                .then((res) => setDestination(res.data.destination))
                .catch((error) => {
                    setError(error.message);
                });
        }
        getData();
    }, [shortAddress]);

    console.log(shortAddress);

    useEffect(() => {
        if (destination) {
            window.location.replace(destination);
        }
    }, [destination]);

    if (!destination && !error) {
        return (
            <Box
                height="100%"
                display="flex"
                alignItems="center"
                justifyContent="center"
            >
                <Spinner />
            </Box>
        );
    }

    return <p>{error && JSON.stringify(error)}</p>;
}

export default HandleRedirectContainer;