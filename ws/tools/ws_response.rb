#
# Standard response for the Webservice
#
class WsResponse

  attr_accessor :code
  attr_accessor :error
  attr_accessor :data

  def initialize
    code = ErrorCodes::OK
  end

  def to_json(*a)
    json = "{"
    json += "\"c\": #{@code},"
    if error != nil
      json += "\"e\": \"#{@error}\","
    end
    if data != nil
      json += "\"r\": #{@data}"
    end
    json += "}"
  end

  # Return the json response, encrypted or not
  def to_output

    json = self.to_json

    if $io_encryption
      return Encryption::encrypt(json)
    else
      return json
    end
  end

end
