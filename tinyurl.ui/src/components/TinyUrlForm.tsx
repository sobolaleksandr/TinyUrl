import { Input, Button, Box, Heading, InputGroup } from "@chakra-ui/react"
import axios from "axios";
import React, { useState } from 'react'
import { SERVER_ENDPOINTS } from "../config";

function TinyUrlForm() {
    const [fullUrl, setDestination] = useState();

    async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        const result = await axios
            .post(`${SERVER_ENDPOINTS}`, null, {
                params:
                {
                    fullUrl
                }
            })
            .then((resp) => resp.data);

        console.log(result);
    }

    return (
        <Box pos="relative">
            <form onSubmit={handleSubmit}>
                destination : {fullUrl}
                <Input
                    onChange={(e: any) => setDestination(e.target.value)}
                    placeholder="https://example.com"
                />
                <Button type="submit">CREATE</Button>
            </form>
        </Box>
    );
}

export default TinyUrlForm;