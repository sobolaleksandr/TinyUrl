import { Input, Button, Box, InputGroup } from "@chakra-ui/react"
import axios from "axios";
import React, { useState } from 'react'
import { SERVER_ENDPOINTS } from "../config";

function TinyUrlForm() {
    const [fullUrl, setDestination] = useState();
    const [shortUrl, setShortUrl] = useState<{
        shortAddress: string;
    } | null>(null);

    const [qrCodePath, setImagePath] = useState();

    async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        setShortUrl(null);
        const result = await axios
            .post(`${SERVER_ENDPOINTS}`, null, {
                params:
                {
                    fullUrl
                }
            })
            .then((resp) => resp.data);

        setShortUrl(result);
        setImagePath(result.qrCodePath);
        console.log(result);
    }

    return (
        <Box pos="relative" zIndex="2" backgroundColor="white" padding="6">
            <form onSubmit={handleSubmit}>
                <InputGroup>
                    <Input
                        onChange={(e: any) => setDestination(e.target.value)}
                        placeholder="https://example.com"
                    />
                    <Button type="submit">Создать</Button>
                </InputGroup>
            </form>
            {shortUrl && (
                <div>
                    <a href={`/${shortUrl?.shortAddress}`}>
                        {window.location.origin}/{shortUrl?.shortAddress}
                    </a>
                    <img src={process.env.PUBLIC_URL + qrCodePath} alt="QR-код"/>
                </div>
            )}
        </Box>
    );
}

export default TinyUrlForm;