# Sample C# File Upload and Send Message integrated with eCourtDate API

This README provides instructions and information about a sample CSharp application that demonstrates how to use the eCourtDate API to upload a file (test PDF included) and then send a one-off omnichannel message.

## eCourtDate Overview

eCourtDate is an omnichannel and multilingual messaging system. Check out our [getting started page for developers here](https://devs.ecourtdate.com).

### Features
- **Omnichannel Support**: Seamlessly integrate to send and receive SMS, MMS, Voice Calls, Emails, Chats, and Push Notifications.
- **Multilingual Capabilities**: Automatically detect and translate messages to and from multiple languages, facilitating access to information.
- **Easy Integration**: Simple and clear API endpoints for sending and receiving messages.
- **Customizable**: Extensible architecture allowing for in-app users to customize templates.

## Getting Started

### Prerequisites
- .NET Core 3.1 or later
- Visual Studio or any compatible IDE
- Git

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/ecourtdate/demo-csharp-files.git
   cd demo-csharp-files
   ```

2. **Open the Solution**
   Open the `.sln` file in Visual Studio or your preferred IDE.

3. **Restore NuGet Packages**
   Ensure all required NuGet packages are restored for the project to build successfully.

4. **Set Up Configuration**
   Get your necessary API keys from the [eCourtDate Console APIs page](https://console.ecourtdate.com). Ensure to set your credentials locally.

5. **Build and Run**
   Build the solution and run the project. The API should now be up and running on your local development server. `dotnet run`

### Usage

- **Sending Messages**
  Send messages through any supported channel by making a POST request to `/v1/messages/oneoffs` with the required payload.

- **Receiving Messages**
  Set up webhooks to handle incoming messages. Read our [Webhooks Guide here](https://webhooks.ecourtdate.com).

- **Translation**
  Utilize the `/v1/translator` endpoint to translate messages to and from supported languages.

- **Synthesizer**
  Utilize the `/v1/synthesize` endpoint to generate text to speech audio files with gender, language, and voice controls.

## Contributing

We welcome contributions from the community! Whether it's submitting bug reports, feature requests, or code contributions, your help is valuable. Please email help@ecourtdate.com with any suggestions or check out our [Help Center here](https://help.ecourtdate.com).

## License

This project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code as per the license terms.