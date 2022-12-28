import { Input, Button, Box, InputGroup } from "@chakra-ui/react"
import axios from "axios";
import React, { useState } from 'react'
import { SERVER_ENDPOINTS } from "../config";

function TinyUrlForm() {
    const [fullUrl, setDestination] = useState();
    const [shortUrl, setShortUrl] = useState<{
        shortAddress: string;
        qrCodePath: string;
    } | null>(null);

    const [error, setError] = useState<{
        message: string;
    } | null>(null);

    async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        setShortUrl(null);
        setError(null);
        if (!fullUrl) {
            setError({ message: 'Заполните адрес' })
            return;
        }
        
        const result = await axios
            .post(`${SERVER_ENDPOINTS}`, null, {
                params:
                {
                    fullUrl
                }
            })
            .then((resp) => resp.data)
            .catch((error) => {
                if (error.code === "ERR_NETWORK")
                    setError({ message: 'Сервер не доступен' })
                else
                    setError(error.response.data);

                console.log(error);
            });

        setShortUrl(result);
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
                    <img src={process.env.PUBLIC_URL + shortUrl?.qrCodePath} alt="QR-код" />
                </div>
            )}
            <p>{JSON.stringify(error?.message)}</p>
        </Box>
    );
}

export default TinyUrlForm;